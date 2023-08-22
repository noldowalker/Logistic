using Domain.Models;
using Domain.WorkResults;

namespace Logistic.Application.WorkResult;

public class BusinessResult<T> : IActionResult<T> where T : BaseModel
{
    public List<T> Data { get; init; }
    public List<ActionMessage> Messages { get; init; }
    public bool IsSuccessful { get; init; }
    
    public BusinessResult(List<T> data, List<ActionMessage> messages, bool isSuccessful)
    {
        Data = data;
        Messages = messages;
        IsSuccessful = isSuccessful;
    }

    public BusinessResult(IActionResult<T> actionResult)
    {
        Data = actionResult.Data;
        Messages = actionResult.Messages;
        IsSuccessful = actionResult.IsSuccessful;
    }
}