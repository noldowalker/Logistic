using Domain.Models;
using Domain.WorkResults;

namespace Domain.Interfaces;

public interface IBaseModelsRepository<T>: IDisposable where T : BaseModel
{
    public IActionMessageContainer Results { get; set; }
    IActionResult<T> GetList();
    IActionResult<T> Get(long id);
    Task<IActionResult<T>> Create(T entity);
    Task<IActionResult<T>> Update(T entity);
    Task<IActionResult<T>> Delete(long id);
    Task SaveAsync();
}