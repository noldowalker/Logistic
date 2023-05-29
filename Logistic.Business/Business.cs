using System.Reflection;
using Domain.Attributes;
using Domain.Interfaces;
using Domain.Models;
using Logistic.Application.Services;
using Logistic.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Application;

public static class Business
{
    public static void AddBusinessDependencies(this IServiceCollection services)
    {
        services.AddScoped<CustomerService, CustomerService>();
    }
    
    public static void AddBusinessGeneration(this IServiceCollection services)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Logistic.Core");
        var autoGeneratableTypes = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(AutoGenerateAttribute), true).Any());

        foreach (var type in autoGeneratableTypes)
        {
            var baseBusinessServiceType = typeof(BaseBusinessService<>).MakeGenericType(type);
            var implementationType =
                Assembly.GetExecutingAssembly().GetType($"{type.Namespace}.Services.{type.Name}Service");

            services.AddScoped(baseBusinessServiceType, implementationType);
        }

    }
}