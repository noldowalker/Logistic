using System.Reflection;
using Domain.WorkResults;
using Logistic.Application.WorkResult;
using Logistic.Controllers;
using Logistic.FeatureProviders;
using Logistic.Interfaces;
using Logistic.WorkResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logistic;

public static class Presentation
{
    private static readonly List<Type> BusinessTypesForResultContainer = new List<Type>()
    {
        typeof(Controller),
        typeof(ControllerBase),
        typeof(IActionFilter),
    };
    
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<IPresentationActionMessageContainer, PresentationMessagesContainer>();
    }
    
    // ToDo: понять когда и на каком этапе создается или пересоздается контроллер и чо тут воще происходит.
    public static void AddControllersGeneration(this IServiceCollection services)
    {
        services
            .AddMvc(o => 
                o.Conventions.Add(new GenericControllerRouteConvention())) // ToDo: Разобраться что такое конвенции.
                    .ConfigureApplicationPartManager(m =>
                        m.FeatureProviders.Add(new BaseControllerGenerationFeatureProvider(new[]
                        {
                            Assembly.GetEntryAssembly()
                                .FullName
                        }))
                    );
    } 
}