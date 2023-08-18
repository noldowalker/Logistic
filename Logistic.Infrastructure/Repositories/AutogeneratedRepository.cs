﻿using Domain.Interfaces;
using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Infrastructure.Repositories;

public class AutogeneratedRepository<T> : BaseModelsRepository<T> where T : BaseModel
{
    private readonly IServiceProvider _serviceProvider;

    public AutogeneratedRepository(
        DataBaseContext db, 
        IEnumerable<IInterceptable<T>> interceptors, 
        IServiceProvider serviceProvider, 
        IWorkResult result) : base(db, interceptors, result, serviceProvider)
    { }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected override IEnumerable<T> GetListAction()
    {
        var result = _db.Set<T>().ToList();

        if (result.Count > 0)
            IncludeForeignKeys();
        
        return result;
    }

    protected override T? GetAction(long id)
    {
        var entity =  _db.Set<T>().Find(id);

        if (entity != null)
        {
            IncludeForeignKeys();
        }
        
        return entity;
    }

    protected override bool CreateAction(T item)
    {
        _db.Set<T>().Add(item);

        return true;
    }

    protected override bool UpdateAction(T item)
    {
        UpdateForeignKeys(item);
        _db.Entry(item).State = EntityState.Modified;
        
        return true;
    }

    protected override  bool DeleteAction(T entity)
    {
        _db.Set<T>().Remove(entity);
        
        return true;
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
    
    private bool disposed = false;
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

    private void IncludeForeignKeys()
    {
        var entityType = typeof(T);
        var baseModelType = typeof(BaseModel);

        foreach (var property in entityType.GetProperties())
        {
            if (baseModelType.IsAssignableFrom(property.PropertyType))
                _db.Set<T>().Include(property.Name).Load();
        }
    }
    
    private void UpdateForeignKeys(T currentEntity)
    {
        var entityType = typeof(T);
        var baseModelType = typeof(BaseModel);

        foreach (var property in entityType.GetProperties())
        {
            if (!baseModelType.IsAssignableFrom(property.PropertyType))
                continue;
            
            var type = typeof(IBaseModelsRepository<>).MakeGenericType(property.PropertyType);
            var propertyRepo = _serviceProvider.GetRequiredService(type);
            if (propertyRepo == null)
                continue;
            
            var method = type.GetMethod("Get", new Type[] { typeof(long) });
            if (method == null)
                continue;
            
            var linkValue = property.GetValue(currentEntity); 
            if (linkValue == null)
                continue;

            var id = ((BaseModel) linkValue).Id;
            
            var result = method.Invoke(propertyRepo, new object[] { id });
            property.SetValue(currentEntity, result);
        }
    }
}