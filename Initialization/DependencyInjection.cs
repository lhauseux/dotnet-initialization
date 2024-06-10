using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace LH.Initialization;

public static class DependencyInjection
{
    public static IServiceCollection AddInitializables(this IServiceCollection services, params Type[] initializables)
    {
        services.AddSingleton<IInitializableList, InitializableList>();

        foreach (var initializable in initializables)
        {
            services.AddSingleton(typeof(IInitializable), initializable);
        }

        return services;
    }
}