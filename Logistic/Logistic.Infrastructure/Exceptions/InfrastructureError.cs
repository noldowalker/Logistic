namespace Logistic.Infrastructure.Exceptions;

public class InfrastructureError : Exception
{
    public string UserMessage { get; init; } 
    public InfrastructureError(string userMessage)
    {
        UserMessage = "Ошибка на инфраструктурном уровне. " + userMessage;
    }
}