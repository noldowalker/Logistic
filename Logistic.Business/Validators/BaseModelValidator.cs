using Domain.Models;
using Domain.WorkResults;
using Logistic.Application.Exceptions;
using Logistic.Application.Interfaces;

namespace Logistic.Application.Validators;

public class BaseModelValidator<T> : IValidatable<T> where T : BaseModel
{
    public IBusinessActionMessageContainer Results { get; }

    public BaseModelValidator(IBusinessActionMessageContainer results)
    {
        Results = results;
    }


    public virtual bool IsValidForCreate(T entity)
    {
        var functions = new List<Func<T, bool>>
        {
            IdMustNotExists,
            NotDeactivate
        };
        
        return functions.TrueForAll(f => f(entity));
    }

    public virtual bool IsValidForUpdate(T entity)
    {
        var functions = new List<Func<T, bool>>
        {
            IdMustExists,
            NotDeactivate
        };
        
        return functions.TrueForAll(f => f(entity));
    }

    public virtual bool IsValidForDelete(T entity)
    {
        var functions = new List<Func<T, bool>>
        {
            IdMustExists,
            ExpectDeactivation
        };
        
        return functions.TrueForAll(f => f(entity));
    }

    private bool IdMustExists(T entity)
    {
        if (entity.Id >= 1) 
            return true;
        
        Results.AddError(new ValidationError("При редактировании сущности необходимо допустимый id, который больше 0"));
        return false;

    }
    
    private bool IdMustNotExists(T entity)
    {
        if (entity.Id == 0)
            return true;
        
        Results.AddError(new ValidationError("При создании сущности недопустимо указывать id"));
        return false;
    }

    private bool NotDeactivate(T entity)
    {
        if (!entity.Inactive)
            return true;
        
        Results.AddError(new ValidationError("При редактировании сущности недопустима ее деактивация, используйте для этого удаление"));
        return false;
    }

    private bool ExpectDeactivation(T entity)
    {
        if (entity.Inactive)
            return true;
        
        Results.AddError(new ValidationError("При деактивации допустимо только изменение поля активированности на false"));
        return false;
    }
}