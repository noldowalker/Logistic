using Domain.WorkResults;

namespace Logistic.Application.WorkResult;

public class BusinessResultMessage : ResultMessage
{
    public BusinessResultMessage(string text) : base(text)
    {
    }

    public BusinessResultMessage(string errorUserText, Exception exception) : base(errorUserText, exception)
    {
    }
}