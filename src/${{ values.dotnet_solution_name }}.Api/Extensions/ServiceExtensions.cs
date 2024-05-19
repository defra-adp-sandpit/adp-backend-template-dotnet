using ${{ values.dotnet_solution_name }}.Core.Interfaces;
using ${{ values.dotnet_solution_name }}.Core.Services;

namespace ${{ values.dotnet_solution_name }}.Api.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IItemService, ItemService>();
        return services;
    }
}
