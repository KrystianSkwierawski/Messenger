using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
