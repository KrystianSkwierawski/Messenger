using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Messenger.Areas.Identity.IdentityHostingStartup))]
namespace Messenger.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}