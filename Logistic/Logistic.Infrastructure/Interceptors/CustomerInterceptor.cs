using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Exceptions;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

[Order(1)]
public class CustomerInterceptor : IInterceptable<Customer>
{
    public CustomerInterceptor(IInfrastructureActionMessageContainer resultses)
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
        return true;
    }

    public bool BeforeCreate(Customer entity)
    {
        if (entity.Name.ToLower() != "василий")
            return true;

        Resultses.AddError(new InfrastructureError("Васям тут не место!"));
        return false;
    }

    public bool AfterCreate(Customer entity)
    {
        return true;
    }

    public bool BeforeUpdate(Customer entity)
    {
        if (entity.Name.ToLower() != "василий")
            return true;

        Resultses.AddError(new InfrastructureError("Васям тут не место!"));
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