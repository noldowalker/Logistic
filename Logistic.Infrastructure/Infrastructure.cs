﻿using System.Reflection;
using Domain.Attributes;
using Domain.Interfaces;
using Domain.Models;
using Domain.WorkResults;
using Logistic.Infrastructure.Interfaces;
using Logistic.Infrastructure.Repositories;
using Logistic.Infrastructure.WorkResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Infrastructure;

public static class Infrastructure
{
    public static void AddDbContext(this IServiceCollection services, string connectionString, string migrationsAssembly)
    {
        services.AddDbContext<DataBaseContext> (o => o.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));
    }

    private static readonly List<Type> InfrastructureTypesForResultContainer = new List<Type>()
    {
        typeof(IBaseModelsRepository<>),
        typeof(IInterceptable<>),
    };
        
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddScoped<IBaseModelsRepository<Customer>, CustomersRepository>();
        services.AddScoped<IInfrastructureActionMessageContainer, InfrastructureMessagesContainer>();
    }

    public static void AddInfrastructureGeneration(this IServiceCollection services)
    {
        AddAutogeneratedRepositories(services);
        AddInterceptors(services);
    }

    /// <summary>
    /// Добавляет для всех моделей с аттрибутом AutoGenerateAttribute по репозиторию.
    /// </summary>
    /// <param name="services"></param>
    private static void AddAutogeneratedRepositories(IServiceCollection services)
    {
        var autogeneratableTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetCustomAttributes(typeof(AutoGenerateAttribute), true).Any());

        foreach (var type in autogeneratableTypes)
        {
            var baseBusinessServiceType = typeof(AutogeneratedRepository<>).MakeGenericType(type);
            var baseBusinessInterfaceType = typeof(IBaseModelsRepository<>).MakeGenericType(type);

            services.AddScoped(baseBusinessInterfaceType, baseBusinessServiceType);
        }
    }

    /// <summary>
    /// Ищет в сборке все реализации интерфейса перехватчика действий и добавляет их в Scope
    /// </summary>
    /// <param name="services"></param>
    private static void AddInterceptors(IServiceCollection services)
    {
        var baseType = typeof(BaseModel);
        // ToDo: проверить что лучше, подгружать конкретную сборку через Load или искать по всем AppDomain.CurrentDomain.GetAssemblies()
        var assembly = Assembly.Load("Logistic.Domain");
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).ToList();
        types.Add(baseType);
        
        foreach (var type in types)
        {
            AddInterceptorsForType(services, type);
        }
    }
    
    private static void AddInterceptorsForType(IServiceCollection services, Type baseModelType)
    {
        // тут важно искать по конкретному типу, т.е. указать какой именно дженерик нас интересует. Иначе в сборке не найдет.
        var interfaceType = typeof(IInterceptable<>).MakeGenericType(baseModelType); 
        var interceptorTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => interfaceType.IsAssignableFrom(t));
        
        foreach (var type in interceptorTypes)
        {
            services.AddScoped(interfaceType, type);
        }
    }
}
