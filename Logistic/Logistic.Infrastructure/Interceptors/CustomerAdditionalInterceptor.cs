using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Exceptions;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

[Order(2)]
public class CustomerAdditionalInterceptor : IInterceptBeforeCreate<Customer>, IInterceptBeforeUpdate<Customer>
{
    public CustomerAdditionalInterceptor(IInfrastructureActionMessageContainer resultses)
    {
        Results = resultses;
    }

    public IInfrastructureActionMessageContainer Results { get; }

    public bool BeforeCreate(Customer entity)
    {
        if (entity.Name.ToLower() != "петр")
            return true;

        Results.AddError(new InfrastructureError("Петям тут не место!"));
        return false;
    }
    
    public bool BeforeUpdate(Customer entity)
    {
        if (entity.Name.ToLower() != "петр")
            return true;

        Results.AddError(new InfrastructureError("Петям тут не место!"));
        return false;
    }
}