﻿using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Domain.Interfaces;
using Domain.Models;
using Domain.WorkResults;
using FluentValidation;
using Logistic.Application.Interfaces;
using Logistic.Application.WorkResult;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Application.Services;

public class AutogeneratedService<T> : IBusinessService<T> where T : BaseModel 
{
    public IBusinessActionMessageContainer Results { get; set; }
        
    private readonly IBaseModelsRepository<T> _repository;
    private readonly AbstractValidator<T> _validator;
    private readonly IServiceProvider _serviceProvider;
    
    public AutogeneratedService(
        IBaseModelsRepository<T> repository, 
        IBusinessActionMessageContainer results, 
        IServiceProvider serviceProvider,
        AbstractValidator<T> validator)
    {
        _repository = repository;
        Results = results;
        _serviceProvider = serviceProvider;
        _validator = validator;
    }
    
    public virtual BusinessResult<T> GetList()
    {
        var actionResult = _repository.GetList();
        
        return new BusinessResult<T>(actionResult);
    }
    
    public virtual BusinessResult<T> Get(long id)
    {
        var actionResult = _repository.Get(id);
        
        return new BusinessResult<T>(actionResult);
    }

    public virtual async Task<BusinessResult<T>> Create(List<object> entities)
    {
        var domainEntities = new List<T>();
        foreach (var entity in entities)
        {
            var domainEntity = MapToDomain((JsonElement)entity);
            
            if (domainEntity == null)
                continue;

            var validationResult = _validator.Validate(
                domainEntity, 
                options => options.IncludeRuleSets("Create"));

            if (!validationResult.IsValid)
            {
                Results.ConvertFromValidation(validationResult.Errors);
                continue;
            }
            
            domainEntities.Add(domainEntity);
        }

        var data = new List<T>();
        foreach (var domainEntity in domainEntities)
        {
            //ToDo: всплытие ошибок
            await UpdateLinks(domainEntity);
            var actionResult = await _repository.Create(domainEntity);
            
            Results.Messages.AddRange(actionResult.Messages);
            if (actionResult.IsSuccessful) 
                data.Add(domainEntity);
        }

        return new BusinessResult<T>(data, Results.Messages, !Results.IsBroken);
    }

    public virtual async Task<BusinessResult<T>> Update(List<object> entities)
    {
        var domainEntities = new List<T>();
        foreach (var entity in entities)
        {
            var domainEntity = MapToDomain((JsonElement)entity);
            
            if (domainEntity == null)
                continue;

            var validationResult = _validator.Validate(
                domainEntity, 
                options => options.IncludeRuleSets("Update"));

            if (!validationResult.IsValid)
            {
                Results.ConvertFromValidation(validationResult.Errors);
                continue;
            }
            
            domainEntities.Add(domainEntity);
        }

        var data = new List<T>();
        foreach (var domainEntity in domainEntities)
        {
            //ToDo: всплытие ошибок
            await UpdateLinks(domainEntity);
            var actionResult = await _repository.Update(domainEntity);

            Results.Messages.AddRange(actionResult.Messages);
            if (actionResult.IsSuccessful) 
                data.Add(domainEntity);
        }

        return new BusinessResult<T>(data, Results.Messages, !Results.IsBroken);
    }

    private async Task UpdateLinks(T entity)
    {
        var entityType = typeof(T);
        var baseModelType = typeof(BaseModel);
        
        foreach (var property in entityType.GetProperties())
        {
            if (!baseModelType.IsAssignableFrom(property.PropertyType))
                continue;
            
            var repoType = typeof(IBaseModelsRepository<>).MakeGenericType(property.PropertyType);
            var propertyRepo = _serviceProvider.GetRequiredService(repoType);
            if (propertyRepo == null)
                continue;
            
            var validatorType = typeof(IValidatable<>).MakeGenericType(property.PropertyType);
            var propertyValidator = _serviceProvider.GetRequiredService(validatorType);
            if (propertyValidator == null)
                continue;
            
            var method = repoType.GetMethod("Get", new Type[] { typeof(long) });
            if (method == null)
                continue;
            
            var linkValue = property.GetValue(entity); 
            if (linkValue == null)
                continue;

            MethodInfo getOrCreateMethod = GetType().GetMethod(nameof(GetOrCreatePropertyValue), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo genericMethod = getOrCreateMethod.MakeGenericMethod(property.PropertyType);
            var parameters = new object[] { linkValue, propertyRepo, propertyValidator };
            dynamic task = genericMethod.Invoke(this, parameters);
            var result = await task;
            
            if (result != null)
            {
                property.SetValue(entity, result);
            }
            
            property.SetValue(entity, result);
        }
    }
    
    //ToDo: подумать что делать с возвратом null, эт не очень тема.
    private async Task<TLink?> GetOrCreatePropertyValue<TLink>(
        TLink valueInEntity, 
        IBaseModelsRepository<TLink> repository,
        IValidatable<TLink> validator) where TLink : BaseModel
    {
        TLink? data = null;
        if (valueInEntity.Id > 0)
        {
            if (!validator.IsValidForUpdate(valueInEntity))
                return null;
            
            var result = await repository.Update(valueInEntity);
            if (result.IsSuccessful)
                data = result.Data.First();
        }
        else
        {
            if (!validator.IsValidForCreate(valueInEntity))
                return null;
            
            var result = await repository.Create(valueInEntity);
            if (result.IsSuccessful)
                data = result.Data.First();
        }

        return data;
    }
    
    private T? MapToDomain(JsonElement entity)
    {
        var type = typeof(T);
        var constructor = type.GetConstructor(new Type[] {});
        var result = (T) constructor?.Invoke(new T[] { });
        
        foreach (var field in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var firstChar = char.ToLower(field.Name[0]);
            var modifiedFieldName = firstChar + field.Name.Substring(1);
            var isPropertyInRequest = entity.TryGetProperty(modifiedFieldName, out JsonElement requestProperty);
            
            if (!isPropertyInRequest)
                continue;

            var value = GetValueForPropertyInJson(field, requestProperty);
            field.SetValue(result, value);
        }
        
        return result;
    }

    private object? GetValueForPropertyInJson(PropertyInfo field, JsonElement value)
    {
        var valueKind = value.ValueKind;
        var propertyTypeName = field.PropertyType.Name;
        var propertyType = field.PropertyType;
        if (field.PropertyType.IsSubclassOf(typeof(BaseModel)))
            propertyTypeName = "BaseModelChild";

        switch (propertyTypeName)
        {
            case "Int64":
                return (valueKind == JsonValueKind.Number) ? value.GetInt64() : null;
            case "String":
                return (valueKind == JsonValueKind.String) ? value.GetString() : null;
            case "BaseModelChild": 
                var JSONCovert = typeof(JsonSerializer);
                var parameterTypes = new[] { typeof(JsonElement), typeof(JsonSerializerOptions) };
                var deserializer = JSONCovert
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(i => i.Name.Equals("Deserialize", StringComparison.InvariantCulture))
                    .Where(i => i.IsGenericMethod)
                    .Single(i => i.GetParameters().Select(a => a.ParameterType).SequenceEqual(parameterTypes));
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = new UpperCaseNamingPolicy(),
                    WriteIndented = true
                };
                var genericMethodInfo = deserializer.MakeGenericMethod(propertyType);
                return (valueKind == JsonValueKind.Object) ? genericMethodInfo.Invoke(null, new object[] { value, options }) : null;
            case "Boolean": 
                return valueKind is JsonValueKind.True or JsonValueKind.False ? value.GetBoolean() : null;
            default:
                return null;
        }
    }
}

public class UpperCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) =>
        Char.ToLower(name[0]) + name.Substring(1);
}