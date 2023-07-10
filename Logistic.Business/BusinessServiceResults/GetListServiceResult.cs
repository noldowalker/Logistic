using Domain.Models;
using Logistic.Application.BusinessModels;

namespace Logistic.Application.BusinessServiceResults;

public class GetListServiceResult<T> : BusinessServiceResult where T : BaseModelBusiness
{
    public List<T> ListOfModels { get; set; }

    public GetListServiceResult()
    {
        ListOfModels = new List<T>();
    }
}