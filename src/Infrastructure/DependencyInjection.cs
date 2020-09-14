using Domain.Interfaces;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDateTime, MachineDateTime>();
            services.AddScoped<IContext, Context>();

            return services;
        }
    }
}
