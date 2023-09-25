using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;

public interface IInterceptAfterRead<T> where T : BaseModel
{
    public IInfrastructureActionMessageContainer Results { get; }
    public bool AfterRead(T entity);
}