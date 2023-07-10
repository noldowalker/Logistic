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
    
    public static void AddControllersGeneration(this IServiceCollection services)
    {
        services
            .AddMvc(o =>
                o.Conventions.Add(new GenericControllerRouteConvention()))
                    .ConfigureApplicationPartManager(m =>
                        m.FeatureProviders.Add(new BaseControllerGenerationFeatureProvider(new[]
                        {
                            Assembly.GetEntryAssembly()
                                .FullName
                        }))
                    );
    } 
}