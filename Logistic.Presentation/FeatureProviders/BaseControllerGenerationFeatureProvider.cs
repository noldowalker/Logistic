using System.Reflection;
using Domain.Attributes;
using Logistic.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Logistic.FeatureProviders;

public class BaseControllerGenerationFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private string[] Assemblies { get; }
    
    public BaseControllerGenerationFeatureProvider(string[] assemblies)
    {
        this.Assemblies = assemblies;
    }

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Logistic.Domain");
        var autoGeneratableTypes = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(AutoGenerateAttribute), true).Any());

        foreach (var candidate in autoGeneratableTypes)
        {
            if (candidate.FullName != null && candidate.FullName.Contains("BaseController")) continue;

            var propertyType = candidate.GetProperty("id")
                ?.PropertyType;
            if (propertyType == null) continue;
            var typeInfo = typeof(BaseController<>).MakeGenericType(candidate)
                .GetTypeInfo();

            feature.Controllers.Add(typeInfo);
        }
        
    }
}