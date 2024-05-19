using ADP.Backend.Template.Core.Interfaces;
using ADP.Backend.Template.Core.Services;

namespace ADP.Backend.Template.Api.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IItemService, ItemService>();
        return services;
    }
}
