using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application.Interfaces;

public interface IValidatable<T> where T : BaseModel
{
    public IBusinessActionMessageContainer Results { get; }
    
    public bool IsValidForCreate(T entity);
    public bool IsValidForUpdate(T entity);
    public bool IsValidForDelete(T entity);
}