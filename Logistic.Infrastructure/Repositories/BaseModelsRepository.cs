using Domain.Interfaces;
using Domain.Models;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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


    public IEnumerable<T> GetList()
    {
        //ToDo: придумать способ фильтрации и внедрить, для него должен быть BeforeInterceptor
        var result = GetListAction();
        return AfterGetList(result);
    }

    public T? Get(long id)
    {
        //ToDo: придумать способ фильтрации и внедрить, для него должен быть BeforeInterceptor
        var result = GetAction(id);
        if (AfterGet(result))
            return null;
        
        return result;
    }

    public async Task Create(T item)
    {
        try
        {
            if (!BeforeCreate(item))
                return;
            if (!CreateAction(item))
                return;
            if (!AfterCreate(item))
                return;
            await SaveAsync();
        }
        catch (Exception e)
        {
            ActionRecords
                .Add(WorkRecord.CreateInfrastructureError($"При попытке создания сущности возникла ошибка: {e.Message}", true));
            
            return;
        }

        ActionRecords
            .Add(WorkRecord.CreateNotification("Запись успешно создана!"));
    }

    public virtual async Task Update(T item)
    {
        try
        {
            if (!BeforeUpdate(item))
                return;
            if (!UpdateAction(item))
                return;
            if (!AfterUpdate(item))
                return;
            await SaveAsync();
        }
        catch (Exception e)
        {
            ActionRecords
                .Add(WorkRecord.CreateInfrastructureError($"При попытке обновления сущности возникла ошибка: {e.Message}", true));
            
            return;
        }

        ActionRecords
            .Add(WorkRecord.CreateNotification("Запись успешно обновлена!"));
    }

    public virtual async Task Delete(long id)
    {
        try
        {
            var entity = _db.Set<T>().Find(id);

            if (!BeforeDelete(entity))
                return;
            if (!DeleteAction(entity))
                return;
            if (!AfterDelete(entity))
                return;
            await SaveAsync();
        }
        catch (Exception e)
        {
            ActionRecords
                .Add(WorkRecord.CreateInfrastructureError($"При попытке удаления сущности возникла ошибка: {e.Message}", true));
            
            return;
        }

        ActionRecords
            .Add(WorkRecord.CreateNotification("Запись успешно удалена!"));
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

    
    
    #region Get Virtual Methods

    protected virtual IEnumerable<T> GetListAction()
    {
        return _db.Set<T>().ToList();
    }

    protected virtual IEnumerable<T> AfterGetList(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            var result = AfterGet(entity);
            if (!result)
                return new List<T>();
        }
        
        return entities;
    }
    
    protected virtual T? GetAction(long id)
    {
        return _db.Set<T>().Find(id);
    }

    protected virtual bool AfterGet(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.AfterRead(entity);
            if (result == null)
                continue;
            
            ActionRecords.Add(result);
            if (result.IsChainBreaker)
                return false;
        }

        return true;
    }
    
    #endregion

    #region Create Virtual Methods

    protected virtual bool BeforeCreate(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.BeforeCreate(entity);
            if (result == null)
                continue;
            
            ActionRecords.Add(result);
            if (result.IsChainBreaker)
                return false;
        }

        return true;
    }
    
    protected virtual bool AfterCreate(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.AfterCreate(entity);
            if (result == null)
                continue;
            
            ActionRecords.Add(result);
            if (result.IsChainBreaker)
                return false;
        }

        return true;
    }

    protected virtual bool CreateAction(T item)
    {
        _db.Set<T>().Add(item);
        return true;
    }

    #endregion

    #region Update Virtual Methods

    protected virtual bool BeforeUpdate(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.BeforeUpdate(entity);
            if (result == null)
                continue;
            
            ActionRecords.Add(result);
            if (result.IsChainBreaker)
                return false;
        }

        return true;
    }
    
    protected virtual bool AfterUpdate(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.AfterUpdate(entity);
            if (result == null)
                continue;
            
            ActionRecords.Add(result);
            if (result.IsChainBreaker)
                return false;
        }

        return true;
    }

    protected virtual bool UpdateAction(T item)
    {
        _db.Entry(item).State = EntityState.Modified;
        return true;
    }

    #endregion

    #region Delete Virtual Methods

    protected virtual bool BeforeDelete(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.BeforeDelete(entity);
            if (result == null)
                continue;
            
            ActionRecords.Add(result);
            if (result.IsChainBreaker)
                return false;
        }

        return true;
    }
    
    protected virtual bool AfterDelete(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.AfterDelete(entity);
            if (result == null)
                continue;
            
            ActionRecords.Add(result);
            if (result.IsChainBreaker)
                return false;
        }

        return true;
    }

    protected virtual bool DeleteAction(T entity)
    {
        _db.Set<T>().Remove(entity);
        return true;
    }

    #endregion
}