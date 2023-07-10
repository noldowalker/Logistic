using Domain.Models;
using Logistic.Application.BusinessModels;

namespace Logistic.Application.Mappers;

public class CustomerBusinessMapper : IDomainMappable<Customer, CustomerBusiness>
{
    public Customer MapToDomain(CustomerBusiness entity)
    {
        return new Customer()
        {
            id = entity.id ?? 0,
            inactive = entity.inactive ?? false,
            name = entity.name
        };
    }

    public CustomerBusiness MapFromDomain(Customer entity)
    {
        return new CustomerBusiness()
        {
            id = entity.id,
            inactive = entity.inactive,
            name = entity.name
        };
    }
}