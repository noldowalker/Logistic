using System.Transactions;

namespace Logistic.WebApi.Middlewares;

public class TransactionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly List<string> _nonTransactionalActionsNames = new List<string>()
    {
        "get",
        "list", 
    };

    public TransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsTransactionalContext(context))
        {
            using var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted},
                TransactionScopeAsyncFlowOption.Enabled);

            await _next(context);

            var response = context.Response;
            if (response.StatusCode is >= 200 and < 300)
                transactionScope.Complete();
            else
                transactionScope.Dispose();
        }
        else
        {
            await _next(context);
        }
    }

    private bool IsTransactionalContext(HttpContext context)
    {
        // Если действие контроллера из списка исключений монотранзакционность не используем
        var actionName = context.Request.RouteValues["action"]?.ToString()?.ToLower();
        if (actionName != null && _nonTransactionalActionsNames.Any(actionName.Contains))
            return false;

        return true;
    }
    
}