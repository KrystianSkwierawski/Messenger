using Application;
using Application.Common.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IAudioFileBulider, AudioFileBulider>();
            services.AddTransient<IDateTime, MachineDateTime>();
            services.AddScoped<IContext, Context>();

            return services;
        }
    }
}
