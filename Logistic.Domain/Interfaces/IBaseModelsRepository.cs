using Domain.Models;
using Domain.WorkResults;

namespace Domain.Interfaces;

public interface IBaseModelsRepository<T>: IDisposable where T : BaseModel
{
    public IWorkResult Result { get; set; }
    IEnumerable<T> GetList();
    T? Get(long id);
    Task<T?> Create(T entity);
    Task<T?> Update(T entity);
    Task<T?> Delete(long id);
    Task SaveAsync();
}