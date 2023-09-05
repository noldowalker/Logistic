namespace Logistic.Application;

using Mapster;

using MapsterMapper;

using Microsoft.Extensions.DependencyInjection;

internal static class Mapper
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
