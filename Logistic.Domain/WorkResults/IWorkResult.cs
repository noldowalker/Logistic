namespace Domain.WorkResults;

public interface IWorkResult
{
    public List<WorkMessage> Messages { get; }
    public bool IsBroken { get; }
    public void AddInfrastructureErrorMessage(string text);
    public void AddBusinessErrorMessage(string text);
    public void AddNotificationMessage(string text);
    public void AddWarningMessage(string text);
    public void AddValidationErrorMessage(string text);
    public void AddDebugMessage(string text);
    
    public bool IsInternalError();
    public bool IsBadRequest();
}