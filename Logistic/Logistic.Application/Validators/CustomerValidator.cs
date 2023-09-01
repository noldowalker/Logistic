using System.Text.RegularExpressions;
using Domain.Models;
using Domain.WorkResults;
using Logistic.Application.Exceptions;
using Logistic.Application.Interfaces;
using FluentValidation;

namespace Logistic.Application.Validators;

public class CustomerValidator : AbstractValidator<Customer> 
{
    public CustomerValidator()
    {
        Include(new BaseModelValidator<Customer>());
        
        RuleFor(customer => customer.Name)
            .NotNull().WithMessage("Имя не указано!")
            .Matches(@"^[а-яА-Яa-zA-Z\s]+$").WithMessage("Имя может включать в себя только кириллические или латинские символы.");
    }
}