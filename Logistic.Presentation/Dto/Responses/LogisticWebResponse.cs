using Domain.WorkResults;
using Logistic.Application;
using Logistic.Infrastructure.WorkResult;
using Logistic.WorkResult;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Dto.Responses;

public class LogisticWebResponse : ActionResult
{
    public List<object> Data { get; set; }
    public List<ActionMessage> Records { get; set; } = new List<ActionMessage>();
    public List<string> Flags { get; set; } = new List<string>();
    private int _statusCode;
    
    public LogisticWebResponse()
    {
        Data = new List<object>();
        _statusCode = 200;
    }
    
    public LogisticWebResponse(List<object> data)
    {
        Data = data;
        _statusCode = 200;
    }

    public ObjectResult AsObjectResult()
    {
        return new ObjectResult(this)
        {
            StatusCode = _statusCode
        };
    }

    public void MarkAsBadRequest()
    {
        _statusCode = 400;
    }
    
    public void MarkAsInternalError()
    {
        _statusCode = 500;
    }
    

    public static LogisticWebResponse CreateWithNotification(string notificationText, List<ActionMessage> errors)
    {
        var record = new PresentationActionMessage()
        {
            Text = notificationText
        };
        
        var result = new LogisticWebResponse()
        {
            Records = errors
        }; 
        
        result.Records.Add(record);
        return result;
    }
}