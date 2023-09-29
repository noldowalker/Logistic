using Domain.Models;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logistic.Infrastructure.Repositories;

public class CustomersRepository: BaseModelsRepository<Customer>
{
    private readonly IServiceProvider _serviceProvider;
    public CustomersRepository(
        DataBaseContext db, 
        IServiceProvider serviceProvider, 
        IInfrastructureActionMessageContainer results) : base(db, results, serviceProvider)
    {
    }

    protected override List<Customer> GetListAction()
    {
        var result = _db
            .Set<Customer>()
            .Include(c => c.Address)
            .ToList();
        
        // Results.AddNotification("Тестовый все окей!");
        // Results.AddError(new InfrastructureError("Тестовая ошибка инфраструктуры"));
        return result;
    }

    protected override Customer? GetAction(long id)
    {
        return _db.Set<Customer>()
            .Include(c => c.Address)
            .SingleOrDefault(c => c.Id == id);
    }
}