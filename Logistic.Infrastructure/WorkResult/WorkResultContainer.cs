using Domain.Enum;
using Domain.WorkResults;

namespace Logistic.Infrastructure.WorkResult;

public class WorkResultContainer : IWorkResult
{
    public List<WorkMessage> Messages { get; } = new List<WorkMessage>();
    public bool IsBroken { get { return _isBroken; } }
    private bool _isBroken = false;
    
    private static readonly List<WorkRecordLevel> BadRequestLevels = new List<WorkRecordLevel>()
    {
        WorkRecordLevel.ValidationError,
        WorkRecordLevel.BusinessError
    };
    
    private static readonly List<WorkRecordLevel> InternalErrorLevels = new List<WorkRecordLevel>()
    {
        WorkRecordLevel.InfrastructureError
    };
    
    public void AddInfrastructureErrorMessage(string text)
    {
        Messages.Add(WorkMessage.CreateInfrastructureError(text, true));
        _isBroken = true;
    }

    public void AddBusinessErrorMessage(string text)
    {
        Messages.Add(WorkMessage.CreateBusinessError(text, true));
        _isBroken = true;
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
        _isBroken = true;
    }

    public void AddDebugMessage(string text)
    {
        Messages.Add(WorkMessage.CreateDebug(text));
    }

    public bool IsBadRequest()
    {
        return Messages.Any(r => BadRequestLevels.Contains(r.Level) && r.IsChainBreaker);
    }
    public bool IsInternalError()
    {
        return Messages.Any(r => InternalErrorLevels.Contains(r.Level) && r.IsChainBreaker);
    }
}