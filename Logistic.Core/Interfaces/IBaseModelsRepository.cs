using Domain.Models;

namespace Domain.Interfaces;

public interface IBaseModelsRepository<T>: IDisposable where T : BaseModel
{
    IEnumerable<T> GetList();
    T Get(long id);
    void Create(T item);
    void Update(T item);
    void Delete(long id);
    void Save();
}