using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

public class BaseInterceptor : IInterceptAfterRead<BaseModel>
{
    public IInfrastructureActionMessageContainer Results { get; }

    public bool AfterRead(BaseModel entity)
    {
        return !entity.Inactive;
    }
}