using Domain.Interfaces;
using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Logistic.Infrastructure.Extensions;
using Logistic.Infrastructure.WorkResult;
using Logistic.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Infrastructure.Repositories;

public class BaseModelsRepository<T> : IBaseModelsRepository<T> where T : BaseModel
{
    protected List<object> _afterReadInterceptors = new List<object>();
    protected List<object> _beforeCreateInterceptors = new List<object>();
    protected List<object> _afterCreateInterceptors = new List<object>();
    protected List<object> _beforeUpdateInterceptors = new List<object>();
    protected List<object> _afterUpdateInterceptors = new List<object>();
    protected List<object> _beforeDeleteInterceptors = new List<object>();
    protected List<object> _afterDeleteInterceptors = new List<object>();
    
    protected DataBaseContext _db;
    public IActionMessageContainer Results { get; set; }

    public BaseModelsRepository(
        DataBaseContext db, 
        IInfrastructureActionMessageContainer results,  
        IServiceProvider serviceProvider)
    {
        _db = db;
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
        
        if (BeforeCreate(entity) && CreateAction(entity))
        {
            await SaveAsync();
            data.Add(entity);
        }

        AfterCreate(entity);

        //ToDo: подумать над енам с уровнями Result.AddNotification($"Запись успешно {typeof(T)} создана с Id = {entity.Id}!");
        return new InfrastructureResult<T>(data.ToList(), Results.Messages, !Results.IsBroken);
    }

    public virtual async Task<IActionResult<T>> Update(T entity)
    {
        var data = new List<T>();
        
        if (BeforeUpdate(entity) && UpdateAction(entity))
        {
            await SaveAsync();
            data.Add(entity);
        }

        AfterUpdate(entity);

        // ToDo: подумать над енам с уровнями  Result.AddDebugMessage($"Запись {typeof(T)} с Id = {entity.Id} успешно обновлена!");
        return new InfrastructureResult<T>(data.ToList(), Results.Messages, !Results.IsBroken);
    }

    public virtual async Task<IActionResult<T>> Delete(long id)
    {
        var data = new List<T>();
        
        var entity = await _db.Set<T>().FindAsync(id);
        
        if (entity != null && BeforeDelete(entity) && DeleteAction(entity))
        {
            await SaveAsync();
            data.Add(entity);
        }

        AfterDelete(entity);

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
        var result = CallInterceptorFunc(
            entity, 
            _afterReadInterceptors, 
            typeof(IInterceptAfterRead<>), 
            nameof(IInterceptAfterRead<T>.AfterRead));
        
        return result;
    }
    
    #endregion

    #region Create Virtual Methods

    protected virtual bool BeforeCreate(T entity)
    {
        var result = CallInterceptorFunc(
            entity, 
            _beforeCreateInterceptors, 
            typeof(IInterceptBeforeCreate<>), 
            nameof(IInterceptBeforeCreate<T>.BeforeCreate));
        
        return result;
    }
    
    protected virtual bool AfterCreate(T entity)
    {
        var result = CallInterceptorFunc(
            entity, 
            _afterCreateInterceptors, 
            typeof(IInterceptAfterCreate<>), 
            nameof(IInterceptAfterCreate<T>.AfterCreate));
        
        return result;
        
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
        var result = CallInterceptorFunc(
            entity, 
            _beforeUpdateInterceptors, 
            typeof(IInterceptBeforeUpdate<>), 
            nameof(IInterceptBeforeUpdate<T>.BeforeUpdate));
        
        return result;
    }
    
    protected virtual bool AfterUpdate(T entity)
    {
        var result = CallInterceptorFunc(
            entity, 
            _afterUpdateInterceptors, 
            typeof(IInterceptAfterUpdate<>), 
            nameof(IInterceptAfterUpdate<T>.AfterUpdate));
        return result;
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
        var result = CallInterceptorFunc(
            entity, 
            _beforeDeleteInterceptors, 
            typeof(IInterceptBeforeDelete<>), 
            nameof(IInterceptBeforeDelete<T>.BeforeDelete));
        
        return result;
    }
    
    protected virtual bool AfterDelete(T entity)
    {
        var result = CallInterceptorFunc(
            entity, 
            _afterDeleteInterceptors, 
            typeof(IInterceptAfterDelete<>), 
            nameof(IInterceptAfterDelete<T>.AfterDelete));
        
        return result;
    }

    protected virtual bool DeleteAction(T entity)
    {
        _db.Set<T>().Remove(entity);
        return true;
    }

    #endregion

    private void GetAllInterceptors(IServiceProvider provider)
    {
        var typesInheritanceChain = typeof(T).GetInheritanceChainFromBaseModel();

        GetInterceptorsOfType(
            provider, 
            typesInheritanceChain, 
            typeof(IInterceptAfterRead<>),
            _afterReadInterceptors);

        GetInterceptorsOfType(
            provider, 
            typesInheritanceChain, 
            typeof(IInterceptBeforeCreate<>),
            _beforeCreateInterceptors);

        GetInterceptorsOfType(
            provider, 
            typesInheritanceChain, 
            typeof(IInterceptAfterCreate<>),
            _afterCreateInterceptors);
        
        GetInterceptorsOfType(
            provider, 
            typesInheritanceChain, 
            typeof(IInterceptBeforeUpdate<>),
            _beforeUpdateInterceptors);
        
        GetInterceptorsOfType(
            provider, 
            typesInheritanceChain, 
            typeof(IInterceptAfterUpdate<>),
            _afterUpdateInterceptors);
        
        GetInterceptorsOfType(
            provider, 
            typesInheritanceChain, 
            typeof(IInterceptBeforeDelete<>),
            _beforeDeleteInterceptors);
        
        GetInterceptorsOfType(
            provider, 
            typesInheritanceChain, 
            typeof(IInterceptAfterDelete<>),
            _afterDeleteInterceptors);
        
    }

    private void GetInterceptorsOfType(IServiceProvider provider, List<Type> typesInheritanceChain, Type interceptorInterfaceType, List<object> interceptorsContainer)
    {
        foreach (var type in typesInheritanceChain)
        {
            var interceptorType = interceptorInterfaceType.MakeGenericType(type);
            var propertyRepo = provider.GetServices(interceptorType);
            if (!propertyRepo.Any())
                continue;
            
            interceptorsContainer.AddRange(propertyRepo);
        }
        
        if (interceptorsContainer.Any())
            interceptorsContainer.SortByOrder();
    }
    private bool CallInterceptorFunc(T entity, List<object> interceptorsCollection, Type interceptorType, string methodName)
    {
        var result = true;

        foreach (var interceptor in interceptorsCollection)
        {
            var interceptorTypes = interceptor.GetType().GetInterfaces().Where(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == interceptorType);
            
            foreach (var implementedInterface in interceptorTypes)
            {
                var genericType = implementedInterface.GetGenericArguments()[0];
                if (!typeof(BaseModel).IsAssignableFrom(genericType)) 
                    continue;
                
                var method = implementedInterface.GetMethod(methodName);
                if (method == null)
                    continue;
                
                result = (bool)method.Invoke(interceptor, new[] { entity });
                if (result == false)
                    return result;
            }
        }
        
        return result;
    }
}