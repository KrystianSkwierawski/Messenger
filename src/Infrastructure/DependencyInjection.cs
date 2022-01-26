using Application;
using Application.Common.Interfaces;
using Domain.Interfaces;
using Infrastructure.Files;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IAudioFileBuilder, AudioFileBuilder>();
            services.AddTransient<IImageFileBuilder, ImageFileBuilder>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddScoped<IContext, Context>();

            return services;
        }
    }
}
