﻿using Domain.Attributes;
using Domain.Models;
using FluentValidation;
using Logistic.Application.Interfaces;
using Logistic.Application.Services;
using Logistic.Application.Validators;
using Logistic.Application.WorkResult;

namespace LogisticInnostage.Application;

using Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMapper();
        AddBusinessGeneration(services);
        AddBusinessDependencies(services);
        AddValidators(services);
        return services;
    }

    private static void AddBusinessGeneration(IServiceCollection services)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Logistic.Domain");
        var autoGeneratableTypes = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(AutoGenerateAttribute), true).Any());

        foreach (var type in autoGeneratableTypes)
        {
            //var baseBusinessInterfaceType = typeof(IBaseBusinessService<>).MakeGenericType(type);
            var baseBusinessServiceType = typeof(AutogeneratedService<>).MakeGenericType(type);
            /*var implementationType =
                Assembly.GetExecutingAssembly().GetType($"{type.Namespace}.Services.{type.Name}Service");*/

            services.AddScoped(baseBusinessServiceType, baseBusinessServiceType);
            
            AddValidatorsForType(services, type);
        }

    }

    private static void AddBusinessDependencies(IServiceCollection services)
    {
        services.AddScoped<CustomerService, CustomerService>();
        services.AddScoped<IBusinessActionMessageContainer, BusinessMessagesContainer>();

        AddValidators(services);
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddScoped<AbstractValidator<Customer>, CustomerValidator>();
    }
    
    private static void AddValidatorsForType(IServiceCollection services, Type baseModelType)
    {
        var validatorType = typeof(AbstractValidator<>).MakeGenericType(baseModelType); 
        var validatorTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => validatorType.IsAssignableFrom(t));
        
        if (validatorTypes.Any())
        {
            foreach (var type in validatorTypes)
            {
                services.AddScoped(validatorType, type);
            }
        }
        else
        {
            services.AddScoped(validatorType, typeof(BaseModelValidator<>).MakeGenericType(baseModelType));
        }
    }
}


