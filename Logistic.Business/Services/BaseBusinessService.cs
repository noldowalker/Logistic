using Domain.Interfaces;
using Domain.Models;

namespace Logistic.Application.Services;

public class BaseBusinessService<T> : IBaseBusinessService<T> where T : BaseModel 
{
    private readonly IBaseModelsRepository<T> _repository;

    public BaseBusinessService(IBaseModelsRepository<T> repository)
    {
        _repository = repository;
    }
    
    public virtual List<T> GetList()
    {
        return _repository.GetList().ToList();
    }
    
    public virtual T? Get(long id)
    {
        return _repository.Get(id);
    }
}