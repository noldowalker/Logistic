using Logistic.Dto.Responses;
using Logistic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Logistic.WebApi.Middlewares;

public class UnexpectedErrorsMiddleware
{
    private readonly RequestDelegate _next;
    
    public UnexpectedErrorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPresentationActionMessageContainer container)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            container.AddError(e, "Произошла непредвиденная ошибка!");
            var result = new LogisticWebResponse(new List<object>(), container);

            await result
                .AsObjectResult()
                .ExecuteResultAsync(new ActionContext
                {
                    HttpContext = context,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor()
                });
        }
        
    }
}