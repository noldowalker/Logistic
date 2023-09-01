using Domain.Models;
using Domain.WorkResults;
using FluentValidation;
using Logistic.Application.Exceptions;
using Logistic.Application.Interfaces;

namespace Logistic.Application.Validators;

public class BaseModelValidator<T> : AbstractValidator<T> where T : BaseModel
{
    public BaseModelValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(b => b.Id)
                .Must(id => id == 0)
                .WithMessage("Id должен отсутствовать или быть равен 0 для создания сущности.");
            
            RuleFor(b => b.Inactive)
                .NotEqual(true)
                .WithMessage("Поле Inactive может быть установлено в true только при операции удаления");
        });

        RuleSet("Update", () =>
        {
            RuleFor(b => b.Id)
                .NotNull().WithMessage("Id должен быть указан для редактирования сущности")
                .GreaterThan(0).WithMessage("Id должен быть больше 0 при создании сущности");
            
            RuleFor(b => b.Inactive)
                .NotEqual(true)
                .WithMessage("Поле Inactive может быть установлено в true только при операции удаления");
        });
        
        RuleSet("Delete", () =>
        {
            RuleFor(b => b.Inactive)
                .Equal(true)
                .WithMessage("При удалении поле Inactive должно устанавливаться в значение true");
        });
    }
    
}