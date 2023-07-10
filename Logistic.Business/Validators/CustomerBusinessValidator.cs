﻿using System.Text.RegularExpressions;
using Logistic.Application.BusinessModels;

namespace Logistic.Application.Validators;

public class CustomerBusinessValidator : BaseModelBusinessValidator<CustomerBusiness>
{
    public override void ValidateForCreate(CustomerBusiness entity)
    {
        base.ValidateForCreate(entity);
        CheckName(entity);
    }

   
    private void CheckName(CustomerBusiness entity)
    {
        var isMatch = entity.name != null && Regex.IsMatch(entity.name, @"^[а-яА-Я\sa-zA-z]+$");

        if (!isMatch)
            ValidationErrors.Add("Имя может включать в себя только кириллические или латинские символы.");
    }
}