﻿using System.Dynamic;
using System.Reflection;
using Domain.Models;
using Logistic.Application.BusinessModels;

namespace Logistic.Application.Mappers;

public class AutogeneratedMapper<TDomain> where TDomain : BaseModel
{
    public object MapFromDomain(TDomain entity)
    {
        var type = entity.GetType();
        var result = new ExpandoObject() as IDictionary<string, object?>;
        foreach (var field in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!field.CanRead)
                continue;

            result.Add(field.Name, field.GetValue(entity));
        }
        
        return result;
    }
}