using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;

public interface IInterceptAfterUpdate<T> where T : BaseModel
{
    public IInfrastructureActionMessageContainer Results { get; }
    public bool AfterUpdate(T entity);
}