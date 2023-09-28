using Domain.WorkResults;
using FluentValidation.Results;
using Logistic.Application.Exceptions;
using Logistic.Application.Interfaces;

namespace Logistic.Application.WorkResult;

public class BusinessMessagesContainer : IBusinessActionMessageContainer
{
    public List<ResultMessage> Messages { get; } = new List<ResultMessage>();
    public bool IsBroken { get { return _isBroken; } }
    private bool _isBroken = false;

    public void AddNotification(string text)
    {
        var message = new BusinessResultMessage(text);
        
        Messages.Add(message);
    }

    public void AddError(Exception exception, string errorUserText = "")
    {
        if (string.IsNullOrEmpty(errorUserText))
            errorUserText = exception.Message;
        
        var message = new BusinessResultMessage(errorUserText, exception);
        
        Messages.Add(message);
        _isBroken = true;
    }

    public void AddBusinessError(string errorUserText)
    {
        var error = new BusinessError(errorUserText);
        AddError(error);
    }
    
    public void AddInfrastructureResults(List<ResultMessage> results, bool isSuccessful)
    {
        Messages.AddRange(results);
        _isBroken = !isSuccessful;
    }

    public void ConvertFromValidation(List<ValidationFailure> validationFailures)
    {
        foreach (var validationFailure in validationFailures)
        {
            var message = new BusinessResultMessage(
                validationFailure.ErrorMessage,
                new ValidationError(validationFailure.ErrorMessage));
            
            Messages.Add(message);
        }
        
        _isBroken = true;
    }
}