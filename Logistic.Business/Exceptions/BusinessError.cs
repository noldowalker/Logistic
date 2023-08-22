namespace Logistic.Application.Exceptions;

public class BusinessError : Exception
{
    public string UserMessage { get; init; } 
    
    public BusinessError(string userMessage)
    {
        UserMessage = "Ошибка на уровне бизнес-логики. " + userMessage;
    }
}