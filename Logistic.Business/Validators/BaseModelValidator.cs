using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application.Validators;

public class BaseModelValidator<T> : IValidatable<T> where T : BaseModel
{
    public List<WorkMessage> ValidationErrors { get; set; }

    public virtual void ValidateForCreate(T entity)
    {
        ValidationErrors = new List<WorkMessage>();
        IdMustNotExists(entity);
        NotDeactivate(entity);
    }

    public virtual void ValidateForUpdate(T entity)
    {
        ValidationErrors = new List<WorkMessage>();
        IdMustExists(entity);
        NotDeactivate(entity);
    }

    public virtual void ValidateForDelete(T entity)
    {
        ValidationErrors = new List<WorkMessage>();
        IdMustExists(entity);
        ExpectDeactivation(entity);
    }

    private void IdMustExists(T entity)
    {
        switch (entity.id)
        {
            case < 1:
                ValidationErrors
                    .Add(WorkMessage.CreateValidationError("При редактировании сущности необходимо допустимый id, который больше 0", true));
                break;
        }
    }
    
    private void IdMustNotExists(T entity)
    {
        if (entity.id != 0)
            ValidationErrors
                .Add(WorkMessage.CreateValidationError("При создании сущности недопустимо указывать id", true));
    }

    private void NotDeactivate(T entity)
    {
        if(entity.inactive == true)
            ValidationErrors
                .Add(WorkMessage.CreateValidationError("При редактировании сущности недопустима ее деактивация, используйте для этого удаление", true));
    }

    private void ExpectDeactivation(T entity)
    {
        if(entity.inactive != true)
            ValidationErrors
                .Add(WorkMessage.CreateValidationError("При деактивации допустимо только изменение поля активированности на false", true));
    }
}