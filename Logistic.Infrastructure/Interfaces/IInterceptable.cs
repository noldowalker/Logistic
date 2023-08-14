using Domain.Interfaces;
using Domain.Models;

namespace Logistic.Infrastructure.Interfaces;


public interface IInterceptable<T> where T : BaseModel
{
    public WorkRecord? BeforeRead(T entity);
    public WorkRecord? AfterRead(T entity);
    public WorkRecord? BeforeCreate(T entity);
    public WorkRecord? AfterCreate(T entity);
    public WorkRecord? BeforeUpdate(T entity);
    public WorkRecord? AfterUpdate(T entity);
    public WorkRecord? BeforeDelete(T entity);
    public WorkRecord? AfterDelete(T entity);
}