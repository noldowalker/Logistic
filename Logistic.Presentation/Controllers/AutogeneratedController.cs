﻿using Domain.Models;
using Domain.WorkResults;
using Logistic.Application.Services;
using Logistic.Dto.Requests;
using Logistic.Dto.Responses;
using Logistic.Enums;
using Logistic.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AutogeneratedController<T> : Controller where T : BaseModel
{
    private readonly AutogeneratedService<T> _service;
    public IWorkResult Results { get; }
    public AutogeneratedController(AutogeneratedService<T> service, IWorkResult results)
    {
        _service = service;
        Results = results;
    }

    [HttpGet(nameof(List))]
    public LogisticWebResponse List()
    {
        var result = new LogisticWebResponse();
        result.Data = _service.GetList().ConvertToObjectsList();
        result.Flags.Add(Flag.Autogenerated.ToString());
        
        return result;
    }
    
    [HttpGet(nameof(Get))]
    public LogisticWebResponse Get(long id)
    {
        var result = new LogisticWebResponse();
        var entity = _service.Get(id);
        if (entity != null)
            result.Data.Add(entity);
        
        result.Flags.Add(Flag.Autogenerated.ToString());
        
        return result;
    }
    
    [HttpPost(nameof(Create))]
    public async Task<LogisticWebResponse> Create(LogisticWebRequestForAutogeneration form)
    {
        var entities = form.Data;
        var createdEntities = await _service.Create(entities);
        
        Results.AddNotificationMessage("Добавление успешно");
        return new LogisticWebResponse(createdEntities.ConvertToObjectsList());
    }
    
    [HttpPost(nameof(Change))]
    public async Task<LogisticWebResponse> Change(LogisticWebRequestForAutogeneration form)
    {
        var entities = form.Data;
        var changedEntities = await _service.Update(entities);
        
        //Results.AddNotificationMessage("Изменение успешно");
        //Results.AddValidationErrorMessage("Тестовая ошибка валидации");
        //Results.AddInfrastructureErrorMessage("Тестовая внутренняя ошибка");
        //Results.AddBusinessErrorMessage("Тестовая ошибка бизнеса");
        return new LogisticWebResponse(changedEntities.ConvertToObjectsList());
    }
}