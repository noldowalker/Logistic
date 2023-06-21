namespace Logistic.Application.BusinessModels.BaseModel;

public partial class BaseModelBusiness : IValidatable
{
    public List<string> ValidationErrors { get; set; }

    public virtual void ValidateForCreate()
    {
        ValidationErrors = new List<string>();
        IdMustNotExists();
        NotDeactivate();
    }

    public virtual void ValidateForUpdate()
    {
        ValidationErrors = new List<string>();
        IdMustExists();
        NotDeactivate();
    }

    public virtual void ValidateForDelete()
    {
        ValidationErrors = new List<string>();
        IdMustExists();
        ExpectDeactivation();
    }

    private void IdMustExists()
    {
        switch (id)
        {
            case null:
                ValidationErrors.Add("При редактировании сущности необходимо указать id");
                break;
            case < 1:
                ValidationErrors.Add("При редактировании сущности необходимо допустимый id, который больше 0");
                break;
        }
    }
    
    private void IdMustNotExists()
    {
        if (id != null)
            ValidationErrors.Add("При создании сущности недопустимо указывать id");
    }

    private void NotDeactivate()
    {
        if(inactive == true)
            ValidationErrors.Add("При редактировании сущности недопустима ее деактивация, используйте для этого удаление");
    }

    private void ExpectDeactivation()
    {
        if(inactive != true)
            ValidationErrors.Add("При деактивации допустимо только изменение поля активированности на false");
    }
}