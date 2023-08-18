using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application.Validators;

public class BaseModelValidator<T> : IValidatable<T> where T : BaseModel
{
    public IWorkResult Result { get; }

    public BaseModelValidator(IWorkResult result)
    {
        Result = result;
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
        
        Result.AddValidationErrorMessage("При редактировании сущности необходимо допустимый id, который больше 0");
        return false;

    }
    
    private bool IdMustNotExists(T entity)
    {
        if (entity.Id == 0)
            return true;
        
        Result.AddValidationErrorMessage("При создании сущности недопустимо указывать id");
        return false;
    }

    private bool NotDeactivate(T entity)
    {
        if (!entity.Inactive)
            return true;
        
        Result.AddValidationErrorMessage("При редактировании сущности недопустима ее деактивация, используйте для этого удаление");
        return false;
    }

    private bool ExpectDeactivation(T entity)
    {
        if (entity.Inactive)
            return true;
        
        Result.AddValidationErrorMessage("При деактивации допустимо только изменение поля активированности на false");
        return false;
    }
}