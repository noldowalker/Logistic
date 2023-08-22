namespace Logistic.Application.Exceptions;

public class ValidationError : Exception
{
    public string UserMessage { get; init; } 
    
    public ValidationError(string userMessage)
    {
        UserMessage = "Ошибка валидации. " + userMessage;
    }
}