using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application;

public interface IBusinessService<T> where T : BaseModel
{
    public IWorkResult Results { get; set; }
}