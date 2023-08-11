using Domain.Models;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Interceptors;

[Order(1)]
public class CustomerInterceptor : IInterceptable<Customer>
{
    public bool IsChainBreaker { get; } = false;
    public List<string> Errors { get; set; } = new List<string>();
    public List<string> Notifications { get; set; } = new List<string>();

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