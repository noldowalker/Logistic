using Domain.Interfaces;
using Domain.Models;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Infrastructure.Repositories;

public class CustomersRepository: BaseModelsRepository<Customer>
{
    private readonly IServiceProvider _serviceProvider;
    public CustomersRepository(DataBaseContext db, IEnumerable<IInterceptable<Customer>> interceptors, IServiceProvider serviceProvider) : base(db, interceptors)
    {
        _serviceProvider = serviceProvider;
    }

    protected override IEnumerable<Customer> GetListAction()
    {
        var result = _db
            .Set<Customer>()
            .Include(c => c.Address)
            .ToList();
        
        //ActionRecords.Add(WorkRecord.CreateNotification("Тестовый все окей!"));
        //ActionRecords.Add(WorkRecord.CreateInfrastructureError("Тестовая ошибка инфраструктуры"));
        return result;
    }

    protected override Customer? GetAction(long id)
    {
        return _db.Set<Customer>()
            .Include(c => c.Address)
            .SingleOrDefault(c => c.id == id);
    }

    public override async Task  Update(Customer item)
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