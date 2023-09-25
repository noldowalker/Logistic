using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;

public interface IInterceptBeforeUpdate<T> where T : BaseModel
{
    public IInfrastructureActionMessageContainer Results { get; }
    public bool BeforeUpdate(T entity);
}