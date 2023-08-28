using Domain.Models;

namespace Logistic.Infrastructure.Extensions;

public static class BaseModelsListExtension
{
    public static List<object> ConvertToObjectsList<T>(this List<T>? list) where T : BaseModel
    {
        return list?.Cast<object>().ToList() ?? new List<object>();
    }
}