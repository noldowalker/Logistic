using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;

public interface IInterceptBeforeCreate<T> where T : BaseModel
{
    public IInfrastructureActionMessageContainer Results { get; }
    public bool BeforeCreate(T entity);
}