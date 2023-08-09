using Domain.Models;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

public class CustomerAdditionalInterceptor : IInterceptable<Customer>
{
    public string? BeforeRead(Customer entity)
    {
        return null;
    }

    public string? AfterRead(Customer entity)
    {
        entity.Name += " INTERCEPTED 2";
        return null;
    }

    public string? BeforeCreate(Customer entity)
    {
        return null;
    }

    public string? AfterCreate(Customer entity)
    {
        return null;
    }

    public string? BeforeUpdate(Customer entity)
    {
        return entity.Name.ToLower() == "петр" ? "Петям тут тоже не место!" : null;
    }

    public string? AfterUpdate(Customer entity)
    {
        return null;
    }

    public string? BeforeDelete(Customer entity)
    {
        return null;
    }

    public string? AfterDelete(Customer entity)
    {
        return null;
    }
}