using Domain.WorkResults;
using Logistic.Infrastructure.Exceptions;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.WorkResult;

public class InfrastructureMessagesContainer : IInfrastructureActionMessageContainer
{
    public List<ResultMessage> Messages { get; } = new List<ResultMessage>();
    public bool IsBroken { get { return _isBroken; } }
    private bool _isBroken = false;

    public void AddNotification(string text)
    {
        var message = new InfrastructureResultMessage(text);
        
        Messages.Add(message);
    }

    public void AddError(Exception exception, string errorUserText = "")
    {
        if (string.IsNullOrEmpty(errorUserText))
            errorUserText = exception.Message;
        
        if (exception is InfrastructureError infrastructureError)
            errorUserText = infrastructureError.UserMessage;
        
        var message = new InfrastructureResultMessage(errorUserText, exception);
        
        Messages.Add(message);
        _isBroken = true;
    }
}