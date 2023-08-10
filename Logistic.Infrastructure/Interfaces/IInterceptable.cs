using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;

public interface IInterceptable<T> where T : BaseModel
{
    public bool IsChainBreaker { get; }
    public List<string> Errors { get; set; }
    public List<string> Notifications { get; set; }
    
    public string? BeforeRead(T entity);
    public string? AfterRead(T entity);
    public string? BeforeCreate(T entity);
    public string? AfterCreate(T entity);
    public string? BeforeUpdate(T entity);
    public string? AfterUpdate(T entity);
    public string? BeforeDelete(T entity);
    public string? AfterDelete(T entity);
}