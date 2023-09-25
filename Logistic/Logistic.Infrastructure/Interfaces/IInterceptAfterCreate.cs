using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;

public interface IInterceptAfterCreate<T> where T : BaseModel
{
    public IInfrastructureActionMessageContainer Results { get; }
    public bool AfterCreate(T entity);
}