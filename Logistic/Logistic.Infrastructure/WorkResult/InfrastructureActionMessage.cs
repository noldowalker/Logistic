using Domain.WorkResults;

namespace Logistic.Infrastructure.WorkResult;

public class InfrastructureResultMessage : ResultMessage
{
    public InfrastructureResultMessage(string text) : base(text)
    {
    }

    public InfrastructureResultMessage(string errorUserText, Exception exception) : base(errorUserText, exception)
    {
    }
}