using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistic.Infrastructure.Repositories;

public class CustomersRepository: BaseModelsRepository<Customer>, ICustomersRepository
{
    public CustomersRepository(DataBaseContext db) : base(db)
    {
    }
}