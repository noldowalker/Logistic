using System.Collections;
using System.Reflection;
using Domain.Models;
using Logistic.Infrastructure.Attributes;
using Logistic.Infrastructure.Interfaces;

namespace Logistic.Infrastructure.Extensions;

public static class SortInterceptorsListExtension
{
    public static void SortByOrder<T>(this List<T> interceptors) where T : class
    {
        if (!interceptors.Any())
            return;

        interceptors.Sort((x, y) =>
        {
            var xAttr = x.GetType().GetCustomAttribute<OrderAttribute>();
            var yAttr = y.GetType().GetCustomAttribute<OrderAttribute>();
            if (xAttr == null && yAttr == null) 
                return 0;
            if (xAttr == null) 
                return 1;
            if (yAttr == null) 
                return -1;
            return xAttr.Order.CompareTo(yAttr.Order);
        });
    }
}