using Domain.Enum;
using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application;

public interface IValidatable<T> where T : BaseModel
{
    public List<WorkMessage> ValidationErrors { get; set; }
    public bool IsValidationSuccessful { get => ValidationErrors.All(w => w.Level != WorkRecordLevel.ValidationError); } 
    
    public void ValidateForCreate(T entity);
    public void ValidateForUpdate(T entity);
    public void ValidateForDelete(T entity);
}