using Logistic.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Dto.Responses;

public class LogisticWebResponse : StatusCodeResult
{
    public List<object> Data { get; set; }
    public string? Notification { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Flags { get; set; }

    public LogisticWebResponse(int statusCode = 200) : base(statusCode)
    {
        Data = new List<object>();
        Flags = new List<string>();
    }

    public static LogisticWebResponse BadResult(List<string> errors)
    {
        return new LogisticWebResponse(400)
        {
            Errors = errors
        };
    }
}