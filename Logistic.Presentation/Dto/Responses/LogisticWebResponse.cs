using Logistic.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Dto.Responses;

public class LogisticWebResponse
{
    public List<object> Data { get; set; }
    public string? Notification { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Flags { get; set; }
    private int _statusCode;

    public LogisticWebResponse()
    {
        Data = new List<object>();
        Flags = new List<string>();
        _statusCode = 200;
    }

    public ObjectResult AsObjectResult()
    {
        return new ObjectResult(this)
        {
            StatusCode = _statusCode
        };
    }
    
    public static LogisticWebResponse BadResult(List<string> errors)
    {
        return new LogisticWebResponse()
        {
            Errors = errors,
            _statusCode = 400,
        };
    }
}