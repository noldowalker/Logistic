using Domain.Enum;
using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application;

public interface IValidatable<T> where T : BaseModel
{
    public IWorkResult Result { get; }
    
    public bool IsValidForCreate(T entity);
    public bool IsValidForUpdate(T entity);
    public bool IsValidForDelete(T entity);
}