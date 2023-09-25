using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;

public interface IInterceptBeforeDelete<T> where T : BaseModel
{
    public IInfrastructureActionMessageContainer Results { get; }
    public bool BeforeDelete(T entity);
}