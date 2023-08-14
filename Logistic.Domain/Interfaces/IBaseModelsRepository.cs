using Domain.Models;

namespace Domain.Interfaces;

public interface IBaseModelsRepository<T>: IDisposable where T : BaseModel
{
    public List<WorkRecord> ActionRecords { get; set; }
    IEnumerable<T> GetList();
    T Get(long id);
    Task Create(T item);
    Task Update(T item);
    Task Delete(long id);
    Task SaveAsync();
}