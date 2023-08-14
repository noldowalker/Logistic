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
    
    public LogisticWebResponse(List<WorkRecord> records)
    {
        Data = new List<object>();
        _statusCode = 200;
        Records = records;
    }
    
    public LogisticWebResponse(List<object> data, List<WorkRecord> records)
    {
        Data = data;
        _statusCode = 200;
        Records = records;
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
        if (Records.IsBadRequestErrors())
            _statusCode = 400;
        
        if (Records.IsInternalErrors())
            _statusCode = 500;
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