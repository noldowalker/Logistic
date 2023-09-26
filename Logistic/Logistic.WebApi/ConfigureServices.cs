using System.Reflection;
using Domain.WorkResults;
using Logistic.Application.WorkResult;
using Logistic.Controllers;
using Logistic.FeatureProviders;
using Logistic.Interfaces;
using Logistic.WebApi.Middlewares;
using Logistic.WorkResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logistic;

public static class ConfigureServices
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
    
    public static void AddControllersGeneration(this IServiceCollection services)
    {
        services
            // Один из методов расшширения для настройки и добавления сервисов свзянных с MVC
            // Конкретно этот принимает набор опций следуя которым должны добавиться сервисы
            .AddMvc(o => 
                // Работаем с коллекцией конвенций. Конвенция позволяет настраивать поведение без явного указания, а определяет общие правила.
                o.Conventions
                    // Тут соответственно добавляется новая конвенция по генерации контроллеров. На вход в ее apply передается модель контроллера.
                    .Add(new GenericControllerRouteConvention())) // весь этот код нужен чтобы для дженерик контроллеров роут начинался с genericType.Name
            .ConfigureApplicationPartManager(m =>
                /*
                 * настраивает и определяет, какие контроллеры следует добавить в приложение на основе определенных условий.
                 * В данном случае, фич-провайдер сканирует сборку "Logistic.Domain" и находит типы с атрибутом AutoGenerateAttribute,
                 * затем создает контроллеры на основе этих типов и добавляет их в ControllerFeature
                 */
                m.FeatureProviders.Add(new BaseControllerGenerationFeatureProvider(new[]
                {
                    Assembly.GetEntryAssembly()
                        .FullName
                }))
            );
    }
}