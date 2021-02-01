using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.IntegrationTests.Common
{
    public static class ContextFactory
    {
        static string _connectionString = "Server=(LocalDB)\\MSSQLLocalDB;Database=MessengerTestDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static Context Create()
        {
            var options = new DbContextOptionsBuilder<Context>()
                     .UseSqlServer(_connectionString)
                     .Options;

            var context = new Context(options);

            try
            {
                context.Database.EnsureCreated();
            }
            catch
            {
            }

            return context;
        }

        public static void Destroy(Context context)
        {
            try
            {
                context.Database.EnsureDeleted();
            }
            catch
            {

            }

            context.Dispose();
        }
    }
}
