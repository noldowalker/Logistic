﻿using System.Reflection;
using Domain.Attributes;
using Domain.Interfaces;
using Domain.Models;
using Logistic.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logistic.Infrastructure;

public static class Infrastructure
{
    public static void AddDbContext(this IServiceCollection services, string connectionString, string migrationsAssembly)
    {
        services.AddDbContext<DataBaseContext> (o => o.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));
    }

    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddScoped<ICustomersRepository, CustomersRepository>();
    }

    public static void AddInfrastructureGeneration(this IServiceCollection services)
    {
        var autoGeneratableTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetCustomAttributes(typeof(AutoGenerateAttribute), true).Any());

        foreach (var type in autoGeneratableTypes)
        {
            var baseBusinessServiceType = typeof(BaseModelsRepository<>).MakeGenericType(type);
            var baseBusinessInterfaceType = typeof(IBaseModelsRepository<>).MakeGenericType(type);
            var implementationType =
                Assembly.GetExecutingAssembly().GetType($"{type.Namespace}.Services.{type.Name}Service");

            services.AddScoped(baseBusinessInterfaceType, baseBusinessServiceType);
        }

    }
}
