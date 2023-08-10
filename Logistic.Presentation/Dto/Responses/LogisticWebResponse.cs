using Domain.Interfaces;
using Logistic.Application;
using Logistic.Application.BusinessServiceResults;
using Logistic.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Dto.Responses;

public class LogisticWebResponse : ActionResult
{
    public List<object> Data { get; set; }
    public List<WorkRecord> Records { get; set; } = new List<WorkRecord>();
    public List<string> Flags { get; set; } = new List<string>();
    private int _statusCode;
    
    public LogisticWebResponse()
    {
        Data = new List<object>();
        _statusCode = 200;
    }

    public ObjectResult AsObjectResult()
    {
        return new ObjectResult(this)
        {
            StatusCode = _statusCode
        };
    }

    public void ActualizeStatusCodeByRecords()
    {
        if (Records.IsContainErrors())
            _statusCode = 400;
    }
    
    public static LogisticWebResponse BadResult(List<WorkRecord> errors)
    {
        return new LogisticWebResponse()
        {
            _statusCode = 400,
            Records = errors,
        };
    }
    
    public static LogisticWebResponse BadResult(string errorText)
    {
        var record = WorkRecord.CreateBusinessError(errorText);
        var result = new LogisticWebResponse()
        {
            _statusCode = 400
        };
        result.Records.Add(record);
        return result;
    }
    

    public static LogisticWebResponse CreateWithNotification(string notificationText, List<WorkRecord> errors)
    {
        var record = WorkRecord.CreateNotification(notificationText);
        var result = new LogisticWebResponse()
        {
            Records = errors
        }; 
        result.Records.Add(record);
        return result;
    }
    
    
}