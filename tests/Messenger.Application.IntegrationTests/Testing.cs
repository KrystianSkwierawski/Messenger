using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;
        private static Checkpoint _checkpoint;
        private static ApplicationUser _currentUser;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
              w.EnvironmentName == "Development" &&
              w.ApplicationName == "Presentation"));

            services.AddLogging();

            startup.ConfigureServices(services);

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };

            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<Context>();

            context.Database.Migrate();
        }

        public static async Task ResetState()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
            _currentUser = null;
        }

        public static async Task<ApplicationUser> CreateUserAsync(string userName, string password, string[] roles)
        {
            using var scope = _scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetService<Microsoft.AspNetCore.Identity.UserManager<IdentityUser>>();

            var user = new ApplicationUser { UserName = userName, Email = userName, Theme = "theme--default" };

            var result = await userManager.CreateAsync(user, password);

            if (roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetService<Microsoft.AspNetCore.Identity.RoleManager<IdentityRole>>();

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRolesAsync(user, roles);
            }

            if (result.Succeeded)
            {
                _currentUser = user;

                return _currentUser;
            }

            throw new Exception();
        }

        public static async Task SignInAsUserAsync(ApplicationUser user)
        {
            using var scope = _scopeFactory.CreateScope();

            var signInManager = scope.ServiceProvider.GetService<SignInManager<IdentityUser>>();

            await signInManager.SignInAsync(user, isPersistent: false);
        }

        public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
           where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<Context>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<Context>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<ISender>();

            var item = await mediator.Send(request);

            return item;
        }
    }
}
