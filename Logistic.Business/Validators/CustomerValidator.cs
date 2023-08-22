using System.Text.RegularExpressions;
using Domain.Models;
using Domain.WorkResults;
using Logistic.Application.Exceptions;
using Logistic.Application.Interfaces;

namespace Logistic.Application.Validators;

public class CustomerValidator : BaseModelValidator<Customer>
{
    public CustomerValidator(IBusinessActionMessageContainer results) : base(results) {}
    
    public override bool IsValidForCreate(Customer entity)
    {
        var result = base.IsValidForCreate(entity);
        return  result && CheckName(entity);
    }

   
    private bool CheckName(Customer entity)
    {
        var isMatch = entity.Name != null && Regex.IsMatch(entity.Name, @"^[а-яА-Я\sa-zA-z]+$");
        if (isMatch)
            return true;
        
        Results.AddError(new ValidationError("Имя может включать в себя только кириллические или латинские символы."));
        return false;
    }
}