using Logistic.Application.BusinessModels;

namespace Logistic.Application.Validators;

public class BaseModelBusinessValidator<T> : IValidatable<T> where T : BaseModelBusiness
{
    public List<string> ValidationErrors { get; set; }

    public virtual void ValidateForCreate(T entity)
    {
        ValidationErrors = new List<string>();
        IdMustNotExists(entity);
        NotDeactivate(entity);
    }

    public virtual void ValidateForUpdate(T entity)
    {
        ValidationErrors = new List<string>();
        IdMustExists(entity);
        NotDeactivate(entity);
    }

    public virtual void ValidateForDelete(T entity)
    {
        ValidationErrors = new List<string>();
        IdMustExists(entity);
        ExpectDeactivation(entity);
    }

    private void IdMustExists(T entity)
    {
        switch (entity.id)
        {
            case null:
                ValidationErrors.Add("При редактировании сущности необходимо указать id");
                break;
            case < 1:
                ValidationErrors.Add("При редактировании сущности необходимо допустимый id, который больше 0");
                break;
        }
    }
    
    private void IdMustNotExists(T entity)
    {
        if (entity.id != null)
            ValidationErrors.Add("При создании сущности недопустимо указывать id");
    }

    private void NotDeactivate(T entity)
    {
        if(entity.inactive == true)
            ValidationErrors.Add("При редактировании сущности недопустима ее деактивация, используйте для этого удаление");
    }

    private void ExpectDeactivation(T entity)
    {
        if(entity.inactive != true)
            ValidationErrors.Add("При деактивации допустимо только изменение поля активированности на false");
    }
}