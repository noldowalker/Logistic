using Logistic.Application.BusinessModels;

namespace Logistic.Dto.Requests;

public class LogisticWebRequestWithEntityList<T> where T : BaseModelBusiness
{
    public List<T> Data { get; set; }
}