using System.Text.Json.Serialization;

namespace Domain.WorkResults;

public class ResultMessage
{
    public string Text { get; set; }
    [JsonIgnore]
    public Exception Error { get; set; }
    public string? ErrorMessage => _errorMessage; 
    public string? StackTrace => _stackTrace;
    
    private string? _errorMessage;
    private string? _stackTrace;

    public ResultMessage(string text)
    {
        Text = text;
        Error = null;
    }

    public ResultMessage(string errorUserText, Exception exception)
    {
        Text = errorUserText;
        Error = exception;
        _errorMessage = exception.Message;
        _stackTrace = exception.StackTrace;
    }
}