using Domain.Models;

namespace Logistic.Application.BusinessServiceResults;

public class GetServiceResult<T> : BusinessServiceResult where T : BaseModel
{
    public T? Model { get; set; }
}