namespace Domain.Enum;

public enum WorkRecordLevel
{
    Undefined = 0,
    
    Notification = 1,
    Warning = 2,
    Debug = 3,
    ValidationError = 4,
    BusinessError = 5,
    InfrastructureError = 6,
}