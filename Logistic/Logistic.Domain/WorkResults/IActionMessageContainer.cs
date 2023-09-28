namespace Domain.WorkResults;

public interface IActionMessageContainer
{
    public List<ResultMessage> Messages { get; }
    public bool IsBroken { get; }

    public void AddNotification(string text);
    public void AddError(Exception exception, string errorUserText = "");
}