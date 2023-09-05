using Domain.Models;

namespace Logistic.Domain.Extensions;

public static class TypeListExtension
{
    public static List<Type> GetInheritanceChainFromBaseModel(this Type derivedType)
    {
        var baseType = typeof(BaseModel);
        var chain = new List<Type>();
        if (!derivedType.IsAssignableTo(baseType))
            return chain;

        var currentType = derivedType;

        while (currentType != baseType)
        {
            chain.Add(currentType);
            currentType = currentType.BaseType;
        }
        
        chain.Add(currentType);
        chain.Reverse();

        return chain;
    }
}