using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;

public interface IInterceptAfterDelete<T> where T : BaseModel
{
    public IInfrastructureActionMessageContainer Results { get; }
    public bool AfterDelete(T entity);
}