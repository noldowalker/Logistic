using System.Text.RegularExpressions;
using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application.Validators;

public class CustomerValidator : BaseModelValidator<Customer>
{
    public CustomerValidator(IWorkResult result) : base(result) {}
    
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
        
        Result.AddValidationErrorMessage("Имя может включать в себя только кириллические или латинские символы.");
        return false;
    }
}