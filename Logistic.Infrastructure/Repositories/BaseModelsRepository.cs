using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistic.Infrastructure.Repositories;

public class BaseModelsRepository<T> : IBaseModelsRepository<T> where T : BaseModel
{
    private DataBaseContext _db;

    public BaseModelsRepository(DataBaseContext db)
    {
        _db = db;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<T> GetList()
    {
        return _db.Set<T>().ToList();
    }

    public T? Get(long id)
    {
        //ToDo: поработать с ошибками
        return _db.Set<T>().Find(id);
    }

    public void Create(T item)
    {
        _db.Set<T>().Add(item);
    }

    public void Update(T item)
    {
        _db.Entry(item).State = EntityState.Modified;
    }

    public void Delete(long id)
    {
        var entity = _db.Set<T>().Find(id);
        if (entity != null)
            _db.Set<T>().Remove(entity);
    }

    public void Save()
    {
        _db.SaveChanges();
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