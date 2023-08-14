using Domain.Interfaces;
using Domain.Models;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

[Order(1)]
public class CustomerInterceptor : IInterceptable<Customer>
{
    public WorkRecord? BeforeRead(Customer entity)
    {
        return null;
    }

    public WorkRecord? AfterRead(Customer entity)
    {
        entity.Name += " INTERCEPTED 1";
        return null;
    }

    public WorkRecord? BeforeCreate(Customer entity)
    {
        if (entity.Name.ToLower() != "василий")
            return null;

        return WorkRecord.CreateInfrastructureError("Васям тут не место!", true);
    }

    public WorkRecord? AfterCreate(Customer entity)
    {
        return null;
    }

    public WorkRecord? BeforeUpdate(Customer entity)
    {
        if (entity.Name.ToLower() != "василий")
            return null;

        return WorkRecord.CreateInfrastructureError("Васям тут не место!", true);
    }

    public WorkRecord? AfterUpdate(Customer entity)
    {
        return null;
    }

    public WorkRecord? BeforeDelete(Customer entity)
    {
        return null;
    }

    public WorkRecord? AfterDelete(Customer entity)
    {
        return null;
    }
}