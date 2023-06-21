namespace Logistic.Application.BusinessModels.Customer;

public partial class CustomerBusiness : IDomainMappable<Domain.Models.Customer>
{
    public Domain.Models.Customer MapToDomain()
    {
        return new Domain.Models.Customer()
        {
            id = id ?? 0,
            inactive = inactive ?? false,
            name = name
        };
    }
}