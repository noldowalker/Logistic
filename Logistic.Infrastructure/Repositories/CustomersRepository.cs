using Domain.Interfaces;
using Domain.Models;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Infrastructure.Repositories;

public class CustomersRepository: BaseModelsRepository<Customer>, ICustomersRepository
{
    private readonly IServiceProvider _serviceProvider;
    public CustomersRepository(DataBaseContext db, IEnumerable<IInterceptable<Customer>> interceptors, IServiceProvider serviceProvider) : base(db, interceptors)
    {
        _serviceProvider = serviceProvider;
    }

    public override IEnumerable<Customer> GetList()
    {
        var result = _db
            .Set<Customer>()
            .Include(c => c.Address)
            .ToList();

        foreach (var entity in result)
        {
            foreach (var interceptor in _interceptors)
            {
                interceptor.AfterRead(entity);
            }
        }
        
        ActionRecords.Add(WorkRecord.CreateNotification("Тестовый все окей!"));
        ActionRecords.Add(WorkRecord.CreateInfrastructureError("Тестовая ошибка инфраструктуры"));
        return result;
    }

    public override Customer? Get(long id)
    {
        return _db.Set<Customer>()
            .Include(c => c.Address)
            .SingleOrDefault(c => c.id == id);
    }

    public override void Update(Customer item)
    {
        base.Update(item);
        
        var type = typeof(IBaseModelsRepository<>).MakeGenericType(typeof(Address));
        var addressRepo = _serviceProvider.GetRequiredService(type) as IBaseModelsRepository<Address>;
        
        if (item.Address == null || addressRepo == null)
            return;

        var address = addressRepo.Get(item.Address.id);
        item.Address = address;
    }
    
}