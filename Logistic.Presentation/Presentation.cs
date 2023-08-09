using System.Reflection;
using Logistic.Controllers;
using Logistic.FeatureProviders;
using Logistic.Infrastructure.Repositories;
using Logistic.Interfaces;
using CustomerBusiness = Logistic.Application.BusinessModels.CustomerBusiness;

namespace Logistic;

public static class Presentation
{
    public static void AddPresentationDependencies(this IServiceCollection services)
    {
        
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