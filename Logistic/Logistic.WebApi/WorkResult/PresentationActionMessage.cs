using Domain.WorkResults;

namespace Logistic.WorkResult;

public class PresentationResultMessage : ResultMessage
{
    public PresentationResultMessage(string errorUserText, Exception exception) : base(errorUserText, exception)
    {
    }
    
    public PresentationResultMessage(string errorUserText) : base(errorUserText)
    {
    }
}