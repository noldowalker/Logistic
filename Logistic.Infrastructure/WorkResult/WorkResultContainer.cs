using Domain.WorkResults;

namespace Logistic.Infrastructure.WorkResult;

public class WorkResultContainer : IWorkResult
{
    public List<WorkMessage> Messages { get; } = new List<WorkMessage>();
    public void AddInfrastructureErrorMessage(string text)
    {
        Messages.Add(WorkMessage.CreateInfrastructureError(text, true));
    }

    public void AddBusinessErrorMessage(string text)
    {
        Messages.Add(WorkMessage.CreateBusinessError(text, true));
    }

    public void AddNotificationMessage(string text)
    {
        Messages.Add(WorkMessage.CreateNotification(text));
    }

    public void AddWarningMessage(string text)
    {
        Messages.Add(WorkMessage.CreateWarning(text));
    }

    public void AddValidationErrorMessage(string text)
    {
        Messages.Add(WorkMessage.CreateValidationError(text, true));
    }

    public void AddDebugMessage(string text)
    {
        Messages.Add(WorkMessage.CreateDebug(text));
    }
}