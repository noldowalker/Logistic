using Domain.Models;

namespace Logistic.Application.BusinessServiceResults;

public class GetListServiceResult<T> : BusinessServiceResult where T : BaseModel
{
    public List<T>? ListOfModels { get; set; }
}