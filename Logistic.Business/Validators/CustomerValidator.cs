using System.Text.RegularExpressions;
using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application.Validators;

public class CustomerValidator : BaseModelValidator<Customer>
{
    public override void ValidateForCreate(Customer entity)
    {
        base.ValidateForCreate(entity);
        CheckName(entity);
    }

   
    private void CheckName(Customer entity)
    {
        var isMatch = entity.Name != null && Regex.IsMatch(entity.Name, @"^[а-яА-Я\sa-zA-z]+$");

        if (!isMatch)
            ValidationErrors
                .Add(WorkMessage.CreateValidationError("Имя может включать в себя только кириллические или латинские символы.", true));
    }
}