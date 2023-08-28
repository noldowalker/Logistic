using Domain.Models;

namespace Logistic.Application;

public interface IBaseBusinessService<T> where T : BaseModel
{
    public List<T> GetList();
    public T? Get(long id);
}