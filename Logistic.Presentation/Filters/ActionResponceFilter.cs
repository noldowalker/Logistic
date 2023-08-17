using Domain.WorkResults;
using Logistic.Dto.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logistic.Filters;

public class ActionResponseFilter : IActionFilter
{
    public IWorkResult Results { get; }

    public ActionResponseFilter(IWorkResult results)
    {
        Results = results;
    }

    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not LogisticWebResponse result) 
            return;
        
        result.Records = Results.Messages;

        if(Results.IsBroken && Results.IsBadRequest())
            result.MarkAsBadRequest();
        
        if(Results.IsBroken && Results.IsInternalError())
            result.MarkAsInternalError();
        
        context.Result = result.AsObjectResult();
    }
}