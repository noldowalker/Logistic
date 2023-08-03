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
            Name = entity.name,
            Address = entity.Address
        };
    }

    public CustomerBusiness MapFromDomain(Customer entity)
    {
        return new CustomerBusiness()
        {
            id = entity.id,
            inactive = entity.inactive,
            name = entity.Name,
            Address = entity.Address
        };
    }
}