using Domain.WorkResults;
using Logistic.Application.Interfaces;
using Logistic.Infrastructure.WorkResult;

namespace Logistic.Application.WorkResult;

public class BusinessMessagesContainer : IBusinessActionMessageContainer
{
    public List<ActionMessage> Messages { get; } = new List<ActionMessage>();
    public bool IsBroken { get { return _isBroken; } }
    private bool _isBroken = false;

    public void AddNotification(string text)
    {
        var message = new BusinessActionMessage()
        {
            Text = text
        };
        
        Messages.Add(message);
    }

    public void AddError(Exception exception, string errorUserText = "")
    {
        if (string.IsNullOrEmpty(errorUserText))
            errorUserText = exception.Message;
        
        var message = new BusinessActionMessage()
        {
            Text = errorUserText,
            Error = exception
        };
        
        Messages.Add(message);
        _isBroken = true;
    }
}