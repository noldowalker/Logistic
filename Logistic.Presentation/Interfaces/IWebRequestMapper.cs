using Logistic.Application.BusinessModels;
using Logistic.Dto.Requests;

namespace Logistic.Interfaces;

public interface IWebRequestMapper<T> where T : BaseModelBusiness
{
    public List<T> MapRequestToList(LogisticWebRequestWithEntityList<T> requestWithEntityList);
}