using Logistic.Domain.Constants;

namespace Logistic.Domain.Entities;

using Logistic.Domain.Constants;

public class Product
{
    public Product(string name, Country country)
    {
        Id = Guid.NewGuid();
        Name = name;
        Country = country;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public Country Country { get; set; }
}
