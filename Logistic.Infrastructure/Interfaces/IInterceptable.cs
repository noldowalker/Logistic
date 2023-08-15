using Domain.Interfaces;
using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Infrastructure.Interfaces;


public interface IInterceptable<T> where T : BaseModel
{
    public IWorkResult Results { get; }
    public bool BeforeRead(T entity);
    public bool AfterRead(T entity);
    public bool BeforeCreate(T entity);
    public bool AfterCreate(T entity);
    public bool BeforeUpdate(T entity);
    public bool AfterUpdate(T entity);
    public bool BeforeDelete(T entity);
    public bool AfterDelete(T entity);
}