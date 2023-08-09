using Domain.Models;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

public class CustomerInterceptor : IInterceptable<Customer>
{
    public string? BeforeRead(Customer entity)
    {
        return null;
    }

    public string? AfterRead(Customer entity)
    {
        entity.Name += " INTERCEPTED 1";
        return null;
    }

    public string? BeforeCreate(Customer entity)
    {
        return entity.Name.ToLower() == "василий" ? "Васям тут не место!" : null;
    }

    public string? AfterCreate(Customer entity)
    {
        throw new NotImplementedException();
    }

    public string? BeforeUpdate(Customer entity)
    {
        return entity.Name.ToLower() == "василий" ? "Васям тут не место!" : null;
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