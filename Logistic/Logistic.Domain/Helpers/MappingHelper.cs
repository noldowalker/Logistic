using System.Reflection;
using System.Text.Json;
using Domain.Models;

namespace LogisticInnostage.Domain.Helpers;

public static class MappingHelper
{
    public static T? MapToDomain<T>(JsonElement entity) where T : BaseModel
    {
        var type = typeof(T);
        var constructor = type.GetConstructor(new Type[] {});
        var result = (T) constructor?.Invoke(new T[] { });
        
        foreach (var field in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var firstChar = char.ToLower(field.Name[0]);
            var modifiedFieldName = firstChar + field.Name.Substring(1);
            var isPropertyInRequest = entity.TryGetProperty(modifiedFieldName, out JsonElement requestProperty);
            
            if (!isPropertyInRequest)
                continue;

            var value = GetValueForPropertyInJson(field, requestProperty);
            field.SetValue(result, value);
        }
        
        return result;
    }

    private static object? GetValueForPropertyInJson(PropertyInfo field, JsonElement value)
    {
        var valueKind = value.ValueKind;
        var propertyTypeName = field.PropertyType.Name;
        var propertyType = field.PropertyType;
        if (field.PropertyType.IsSubclassOf(typeof(BaseModel)))
            propertyTypeName = "BaseModelChild";

        switch (propertyTypeName)
        {
            case "Int64":
                return (valueKind == JsonValueKind.Number) ? value.GetInt64() : null;
            case "String":
                return (valueKind == JsonValueKind.String) ? value.GetString() : null;
            case "BaseModelChild": 
                var JSONCovert = typeof(JsonSerializer);
                var parameterTypes = new[] { typeof(JsonElement), typeof(JsonSerializerOptions) };
                var deserializer = JSONCovert
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(i => i.Name.Equals("Deserialize", StringComparison.InvariantCulture))
                    .Where(i => i.IsGenericMethod)
                    .Single(i => i.GetParameters().Select(a => a.ParameterType).SequenceEqual(parameterTypes));
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = new UpperCaseNamingPolicy(),
                    WriteIndented = true
                };
                var genericMethodInfo = deserializer.MakeGenericMethod(propertyType);
                return (valueKind == JsonValueKind.Object) ? genericMethodInfo.Invoke(null, new object[] { value, options }) : null;
            case "Boolean": 
                return valueKind is JsonValueKind.True or JsonValueKind.False ? value.GetBoolean() : null;
            default:
                return null;
            
            
        }
    }
    
    private class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            Char.ToLower(name[0]) + name.Substring(1);
    }
}