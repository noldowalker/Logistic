using Domain.Enum;

namespace Domain.Interfaces;

public class WorkRecord
{
    public string Text { get; set; }
    public WorkRecordLevel Level { get; init; }

    public static WorkRecord CreateInfrastructureError(string errorMessage) => 
        CreateRecord(errorMessage, WorkRecordLevel.InfrastructureError);

    public static WorkRecord CreateBusinessError(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.BusinessError);
    public static WorkRecord CreateNotification(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Notification);
    public static WorkRecord CreateWarning(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Warning);
    public static WorkRecord CreateValidationError(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.ValidationError);
    public static WorkRecord CreateDebug(string errorMessage) =>
        CreateRecord(errorMessage, WorkRecordLevel.Debug);

    private static WorkRecord CreateRecord(string text, WorkRecordLevel level)
    {
        return new WorkRecord()
        {
            Level = level,
            Text = text
        };
    }
}