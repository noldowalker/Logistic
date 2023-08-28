using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

public class BaseInterceptor : IInterceptable<BaseModel>
{
    public IInfrastructureActionMessageContainer Resultses { get; }
    public bool BeforeRead(BaseModel entity)
    {
        return true;
    }

    public bool AfterRead(BaseModel entity)
    {
        return !entity.Inactive;
    }

    public bool BeforeCreate(BaseModel entity)
    {
        return true;
    }

    public bool AfterCreate(BaseModel entity)
    {
        return true;
    }

    public bool BeforeUpdate(BaseModel entity)
    {
        return true;
    }

    public bool AfterUpdate(BaseModel entity)
    {
        return true;
    }

    public bool BeforeDelete(BaseModel entity)
    {
        return true;
    }

    public bool AfterDelete(BaseModel entity)
    {
        return true;
    }
}