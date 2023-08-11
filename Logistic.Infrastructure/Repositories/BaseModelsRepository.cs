using Domain.Interfaces;
using Domain.Models;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using Logistic.Infrastructure.Extensions;

namespace Logistic.Infrastructure.Repositories;

public class BaseModelsRepository<T> : IBaseModelsRepository<T> where T : BaseModel
{
    protected readonly List<IInterceptable<T>> _interceptors;
    
    protected DataBaseContext _db;
    public List<WorkRecord> ActionRecords { get; set; } = new List<WorkRecord>();

    public BaseModelsRepository(DataBaseContext db, IEnumerable<IInterceptable<T>> interceptors)
    {
        _db = db;
        _interceptors = interceptors.ToList();
        _interceptors.SortByOrder();
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    public virtual IEnumerable<T> GetList()
    {
        return _db.Set<T>().ToList();
    }

    public virtual T? Get(long id)
    {
        return _db.Set<T>().Find(id);
    }

    public virtual void Create(T item)
    {
        _db.Set<T>().Add(item);
    }

    public virtual void Update(T item)
    {
        _db.Entry(item).State = EntityState.Modified;
    }

    public virtual void Delete(long id)
    {
        var entity = _db.Set<T>().Find(id);
        if (entity != null)
            _db.Set<T>().Remove(entity);
    }
    
    public virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
        disposed = true;
    }
    
    public virtual async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
    
    protected bool disposed = false;
}