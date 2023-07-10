using Logistic.Application.BusinessModels;

namespace Logistic.Application;

public interface IValidatable<T> where T : BaseModelBusiness
{
    public List<string> ValidationErrors { get; set; }
    
    public void ValidateForCreate(T entity);
    public void ValidateForUpdate(T entity);
    public void ValidateForDelete(T entity);
}