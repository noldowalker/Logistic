using System.Text.Json.Serialization;
using Domain.Enum;

namespace Domain.Interfaces;

public class WorkRecord
{
    public string Text { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WorkRecordLevel Level { get; init; }
    public bool IsChainBreaker { get; init; }

    public static WorkRecord CreateInfrastructureError(string errorMessage, bool isChainBreaker = false) => 
        CreateRecord(errorMessage, WorkRecordLevel.InfrastructureError, isChainBreaker);
    public static WorkRecord CreateBusinessError(string errorMessage, bool isChainBreaker = false) =>
        CreateRecord(errorMessage, WorkRecordLevel.BusinessError, isChainBreaker);
    public static WorkRecord CreateNotification(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Notification);
    public static WorkRecord CreateWarning(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Warning);
    public static WorkRecord CreateValidationError(string errorMessage, bool isChainBreaker = false) =>
        CreateRecord(errorMessage, WorkRecordLevel.ValidationError, isChainBreaker);
    public static WorkRecord CreateDebug(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Debug);

    private static WorkRecord CreateRecord(string text, WorkRecordLevel level, bool isChainBreaker = false)
    {
        return new WorkRecord()
        {
            Level = level,
            Text = text,
            IsChainBreaker = isChainBreaker
        };
    }
}