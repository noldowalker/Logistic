using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistic.Infrastructure.Repositories;

public class BaseModelsRepository<T> : IBaseModelsRepository<T> where T : BaseModel
{
    protected DataBaseContext _db;
    public List<WorkRecord> ActionRecords { get; set; } = new List<WorkRecord>();

    public BaseModelsRepository(DataBaseContext db)
    {
        _db = db;
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

    public virtual async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
    
    private bool disposed = false;
    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
        this.disposed = true;
    }
}