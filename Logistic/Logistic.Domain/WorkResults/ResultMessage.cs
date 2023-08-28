using System.Text.Json.Serialization;

namespace Domain.WorkResults;

public class ActionMessage
{
    public string Text { get; set; }
    public Exception Error { get; set; }
}