using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Exceptions;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

[Order(1)]
public class CustomerInterceptor :  IInterceptBeforeCreate<Customer>, IInterceptBeforeUpdate<Customer>
{
    public CustomerInterceptor(IInfrastructureActionMessageContainer results)
    {
        Results = results;
    }

    public IInfrastructureActionMessageContainer Results { get; }

    public bool BeforeCreate(Customer entity)
    {
        if (entity.Name.ToLower() != "василий")
            return true;

        Results.AddError(new InfrastructureError("Васям тут не место!"));
        return false;
    }

    public bool BeforeUpdate(Customer entity)
    {
        if (entity.Name.ToLower() != "василий")
            return true;

        Results.AddError(new InfrastructureError("Васям тут не место!"));
        return false;
    }
}