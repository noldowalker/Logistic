using Domain.Enum;
using Domain.WorkResults;

namespace Logistic.Application;

public static class WorkRecordListExtension
{
    private static readonly List<WorkRecordLevel> BadRequestLevels = new List<WorkRecordLevel>()
    {
        WorkRecordLevel.ValidationError,
        WorkRecordLevel.BusinessError
    };
    
    private static readonly List<WorkRecordLevel> InternalErrorLevels = new List<WorkRecordLevel>()
    {
        WorkRecordLevel.InfrastructureError
    };
    public static bool IsBadRequestErrors(this List<WorkMessage> records)
    {
        return records.Any(r => BadRequestLevels.Contains(r.Level) && r.IsChainBreaker);
    }
    public static bool IsInternalErrors(this List<WorkMessage> records)
    {
        return records.Any(r => InternalErrorLevels.Contains(r.Level) && r.IsChainBreaker);
    }
}