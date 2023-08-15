using System.Text.Json.Serialization;
using Domain.Enum;

namespace Domain.WorkResults;

public class WorkMessage
{
    public string Text { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WorkRecordLevel Level { get; init; }
    public bool IsChainBreaker { get; init; }

    public static WorkMessage CreateInfrastructureError(string errorMessage, bool isChainBreaker = false) => 
        CreateRecord(errorMessage, WorkRecordLevel.InfrastructureError, isChainBreaker);
    public static WorkMessage CreateBusinessError(string errorMessage, bool isChainBreaker = false) =>
        CreateRecord(errorMessage, WorkRecordLevel.BusinessError, isChainBreaker);
    public static WorkMessage CreateNotification(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Notification);
    public static WorkMessage CreateWarning(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Warning);
    public static WorkMessage CreateValidationError(string errorMessage, bool isChainBreaker = false) =>
        CreateRecord(errorMessage, WorkRecordLevel.ValidationError, isChainBreaker);
    public static WorkMessage CreateDebug(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Debug);

    private static WorkMessage CreateRecord(string text, WorkRecordLevel level, bool isChainBreaker = false)
    {
        return new WorkMessage()
        {
            Level = level,
            Text = text,
            IsChainBreaker = isChainBreaker
        };
    }
}