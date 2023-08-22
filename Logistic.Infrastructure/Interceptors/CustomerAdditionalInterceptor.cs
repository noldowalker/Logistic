using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Exceptions;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

[Order(2)]
public class CustomerAdditionalInterceptor : IInterceptable<Customer>
{
    public CustomerAdditionalInterceptor(IInfrastructureActionMessageContainer resultses)
    {
        Resultses = resultses;
    }

    public IInfrastructureActionMessageContainer Resultses { get; }

    public bool BeforeRead(Customer entity)
    {
        return true;
    }

    public bool AfterRead(Customer entity)
    {
        entity.Name += " INTERCEPTED 2";
        return true;
    }

    public bool BeforeCreate(Customer entity)
    {
        if (entity.Name.ToLower() != "петр")
            return true;

        Resultses.AddError(new InfrastructureError("Петям тут не место!"));
        return false;
    }

    public bool AfterCreate(Customer entity)
    {
        return true;
    }

    public bool BeforeUpdate(Customer entity)
    {
        if (entity.Name.ToLower() != "петр")
            return true;

        Resultses.AddError(new InfrastructureError("Петям тут не место!"));
        return false;
    }

    public bool AfterUpdate(Customer entity)
    {
        return true;
    }

    public bool BeforeDelete(Customer entity)
    {
        return true;
    }

    public bool AfterDelete(Customer entity)
    {
        return true;
    }
}