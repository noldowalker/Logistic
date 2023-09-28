using Domain.Models;
using Domain.WorkResults;

namespace Logistic.WorkResult;

public class PresentationResult<T> : IActionResult<T> where T : BaseModel
{
    public List<T> Data { get; init; }
    public List<ResultMessage> Messages { get; init; }
    public bool IsSuccessful { get; init; }
}