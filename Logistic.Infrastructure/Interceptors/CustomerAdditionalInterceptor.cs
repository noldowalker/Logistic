using Domain.Interfaces;
using Domain.Models;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

[Order(2)]
public class CustomerAdditionalInterceptor : IInterceptable<Customer>
{
    public bool IsChainBreaker { get; } = false;
    public List<string> Errors { get; set; } = new List<string>();
    public List<string> Notifications { get; set; } = new List<string>();

    public WorkRecord? BeforeRead(Customer entity)
    {
        return null;
    }

    public WorkRecord? AfterRead(Customer entity)
    {
        entity.Name += " INTERCEPTED 2";
        return null;
    }

    public WorkRecord? BeforeCreate(Customer entity)
    {
        if (entity.Name.ToLower() != "петр")
            return null;

        return WorkRecord.CreateInfrastructureError("Петям тут не место!", true);
    }

    public WorkRecord? AfterCreate(Customer entity)
    {
        return null;
    }

    public WorkRecord? BeforeUpdate(Customer entity)
    {
        if (entity.Name.ToLower() != "петр")
            return null;

        return WorkRecord.CreateInfrastructureError("Петям тут не место!", true);
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