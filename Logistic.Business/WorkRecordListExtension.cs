using Domain.Enum;
using Domain.Interfaces;
using Logistic.Application.BusinessServiceResults;

namespace Logistic.Application;

public static class WorkRecordListExtension
{
    private static readonly List<WorkRecordLevel> ErrorLevels = new List<WorkRecordLevel>()
    {
        WorkRecordLevel.ValidationError,
        WorkRecordLevel.BusinessError,
        WorkRecordLevel.InfrastructureError
    };
    public static bool IsContainErrors(this List<WorkRecord> records)
    {
        return records.Any(r => ErrorLevels.Contains(r.Level));
    }
}