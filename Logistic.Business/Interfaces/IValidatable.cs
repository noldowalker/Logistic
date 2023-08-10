using Domain.Enum;
using Domain.Interfaces;
using Logistic.Application.BusinessModels;
using Logistic.Application.BusinessServiceResults;

namespace Logistic.Application;

public interface IValidatable<T> where T : BaseModelBusiness
{
    public List<WorkRecord> ValidationErrors { get; set; }
    public bool IsValidationSuccessful { get => ValidationErrors.All(w => w.Level != WorkRecordLevel.ValidationError); } 
    
    public void ValidateForCreate(T entity);
    public void ValidateForUpdate(T entity);
    public void ValidateForDelete(T entity);
}