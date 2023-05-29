using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Controllers;

public class BaseController<T> : Controller where T : BaseModel
{
    private IBaseModelsRepository<T> _repository;

    public BaseController(IBaseModelsRepository<T> repository)
    {
        _repository = repository;
    }

    [HttpGet(nameof(List))]
    public IEnumerable<T> List()
    {
        return _repository.GetList();
    }
}