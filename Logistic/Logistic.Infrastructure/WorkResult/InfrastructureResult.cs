using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Infrastructure.WorkResult;

public class InfrastructureResult<T> : IActionResult<T> where T : BaseModel
{
    public List<T> Data { get; init; }
    public List<ResultMessage> Messages { get; init; }
    public bool IsSuccessful { get; init; }

    public InfrastructureResult(List<T> data, List<ResultMessage> messages, bool isSuccessful)
    {
        Data = data;
        Messages = messages;
        IsSuccessful = isSuccessful;
    }
}