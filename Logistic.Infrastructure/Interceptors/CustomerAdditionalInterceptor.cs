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