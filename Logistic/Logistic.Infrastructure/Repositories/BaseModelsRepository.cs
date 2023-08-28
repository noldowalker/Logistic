using Domain.Interfaces;
using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Exceptions;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Logistic.Infrastructure.Extensions;
using Logistic.Infrastructure.WorkResult;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Infrastructure.Repositories;

public class BaseModelsRepository<T> : IBaseModelsRepository<T> where T : BaseModel
{
    protected readonly List<IInterceptable<T>> _interceptors;
    protected List<object> _interceptors2;
    
    protected DataBaseContext _db;
    public IActionMessageContainer Results { get; set; }

    public BaseModelsRepository(
        DataBaseContext db, 
        IEnumerable<IInterceptable<T>> interceptors, 
        IInfrastructureActionMessageContainer results,  
        IServiceProvider serviceProvider)
    {
        _db = db;
        _interceptors = interceptors.ToList();
        _interceptors.SortByOrder();
        GetAllInterceptors(serviceProvider);
        Results = results;
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    public IActionResult<T> GetList()
    {
        //ToDo: придумать способ фильтрации и внедрить, для него должен быть BeforeInterceptor
        var data = GetListAction();
        data = AfterGetList(data);

        return new InfrastructureResult<T>(data.ToList(), Results.Messages, !Results.IsBroken);
    }

    public IActionResult<T> Get(long id)
    {
        //ToDo: придумать способ фильтрации и внедрить, для него должен быть BeforeInterceptor
        var data = new List<T>();
        var result = GetAction(id);
        if (AfterGet(result))
            data.Add(result);
        
        return new InfrastructureResult<T>(data.ToList(), Results.Messages, !Results.IsBroken);
    }

    public async Task<IActionResult<T>> Create(T entity)
    {
        var data = new List<T>();
        
        try
        {
            if (BeforeCreate(entity) && CreateAction(entity) && AfterCreate(entity))
            {
                await SaveAsync();
                data.Add(entity);
            }
        }
        catch (Exception e)
        {
            Results.AddError(new InfrastructureError($"При попытке создания сущности {typeof(T)} возникла ошибка: {e.Message}"));
        }

        //ToDo: подумать над енам с уровнями Result.AddNotification($"Запись успешно {typeof(T)} создана с Id = {entity.Id}!");
        return new InfrastructureResult<T>(data.ToList(), Results.Messages, !Results.IsBroken);
    }

    public virtual async Task<IActionResult<T>> Update(T entity)
    {
        var data = new List<T>();
        
        try
        {
            if (BeforeUpdate(entity) && UpdateAction(entity) && AfterUpdate(entity))
            {
                await SaveAsync();
                data.Add(entity);
            }
        }
        catch (Exception e)
        {
            Results
                .AddError(new InfrastructureError($"При попытке обновления сущности {typeof(T)} с Id = {entity.Id} возникла ошибка: {e.Message}"));
        }

        // ToDo: подумать над енам с уровнями  Result.AddDebugMessage($"Запись {typeof(T)} с Id = {entity.Id} успешно обновлена!");
        return new InfrastructureResult<T>(data.ToList(), Results.Messages, !Results.IsBroken);
    }

    public virtual async Task<IActionResult<T>> Delete(long id)
    {
        var data = new List<T>();
        
        try
        {
            var entity = await _db.Set<T>().FindAsync(id);
            
            if (entity != null && BeforeDelete(entity) && DeleteAction(entity) && AfterDelete(entity))
            {
                await SaveAsync();
                data.Add(entity);
            }
        }
        catch (Exception e)
        {
            Results
                .AddError(new InfrastructureError($"При попытке удаления сущности {typeof(T)} с Id = {id} возникла ошибка: {e.Message}"));
        }

        // ToDo: подумать над енам с уровнями  Result.AddDebugMessage($"Запись {typeof(T)} с Id = {entity.Id} успешно обновлена!");
        return new InfrastructureResult<T>(data.ToList(), Results.Messages, !Results.IsBroken);
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
        var result = new List<T>();
        foreach (var entity in entities)
        {
            var isEntityMayBeReturned = AfterGet(entity);
            if (isEntityMayBeReturned)
                result.Add(entity);
        }
        
        return result;
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
            if (!result)
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
            if (!result)
                return false;
        }

        return true;
    }
    
    protected virtual bool AfterCreate(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.AfterCreate(entity);
            if (!result)
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
            if (!result)
                return false;
        }

        return true;
    }
    
    protected virtual bool AfterUpdate(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.AfterUpdate(entity);
            if (!result)
                return false;
        }

        return true;
    }

    protected virtual bool UpdateAction(T item)
    {
        /*var type = typeof(IBaseModelsRepository<>).MakeGenericType(typeof(Address));
        var addressRepo = _serviceProvider.GetRequiredService(type) as IBaseModelsRepository<Address>;*/
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
            if (!result)
                return false;
        }

        return true;
    }
    
    protected virtual bool AfterDelete(T entity)
    {
        foreach (var interceptor in _interceptors)
        {
            var result = interceptor.AfterDelete(entity);
            if (!result)
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

    private void GetAllInterceptors(IServiceProvider provider)
    {
        _interceptors2 = new List<object>();
        var types = GetInheritanceChain();
        foreach (var type in types)
        {
            var interceptorType = typeof(IInterceptable<>).MakeGenericType(type);
            var propertyRepo = provider.GetServices(interceptorType);
            if (!propertyRepo.Any())
                continue;
            
            _interceptors2.AddRange(propertyRepo);
        }
        
        if (_interceptors2.Any())
            _interceptors2.SortByOrder();
    } 
    
    private List<Type> GetInheritanceChain()
    {
        Type derivedType = typeof(T);
        Type baseType = typeof(BaseModel);
        List<Type> chain = new List<Type>();

        Type currentType = derivedType;

        while (currentType != null && currentType != baseType)
        {
            chain.Add(currentType);
            currentType = currentType.BaseType;
        }

        if (currentType == baseType)
        {
            chain.Add(currentType);
            chain.Reverse();
        }
        else
        {
            chain.Clear();
        }

        return chain;
    }
}