using Domain.WorkResults;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.WorkResult;

public class InfrastructureMessagesContainer : IInfrastructureActionMessageContainer
{
    public List<ActionMessage> Messages { get; } = new List<ActionMessage>();
    public bool IsBroken { get { return _isBroken; } }
    private bool _isBroken = false;

    public void AddNotification(string text)
    {
        var message = new InfrastructureActionMessage()
        {
            Text = text
        };
        
        Messages.Add(message);
    }

    public void AddError(Exception exception, string errorUserText = "")
    {
        if (string.IsNullOrEmpty(errorUserText))
            errorUserText = exception.Message;
        
        var message = new InfrastructureActionMessage()
        {
            Text = errorUserText,
            Error = exception
        };
        
        Messages.Add(message);
        _isBroken = true;
    }
}