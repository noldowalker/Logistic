using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logistic.Infrastructure.Repositories;

public class CustomersRepository: BaseModelsRepository<Customer>
{
    private readonly IServiceProvider _serviceProvider;
    public CustomersRepository(
        DataBaseContext db, 
        IEnumerable<IInterceptable<Customer>> interceptors, 
        IServiceProvider serviceProvider, 
        IWorkResult result) : base(db, interceptors, result)
    {
        _serviceProvider = serviceProvider;
    }

    protected override IEnumerable<Customer> GetListAction()
    {
        var result = _db
            .Set<Customer>()
            .Include(c => c.Address)
            .ToList();
        
        Result.AddNotificationMessage("Тестовый все окей!");
        Result.AddInfrastructureErrorMessage("Тестовая ошибка инфраструктуры");
        return result;
    }

    protected override Customer? GetAction(long id)
    {
        return _db.Set<Customer>()
            .Include(c => c.Address)
            .SingleOrDefault(c => c.id == id);
    }
}