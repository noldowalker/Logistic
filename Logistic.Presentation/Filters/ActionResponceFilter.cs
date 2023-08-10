using Logistic.Application;
using Logistic.Dto.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logistic.Filters;

public class ActionResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not LogisticWebResponse result) 
            return;
        
        result.ActualizeStatusCodeByRecords();
        context.Result = result.AsObjectResult();
    }
}