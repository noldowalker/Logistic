using Domain.Models;

namespace Logistic.Dto.Requests;

public class LogisticWebRequestWithEntityList<T> where T : BaseModel
{
    public List<T> Data { get; set; }
}