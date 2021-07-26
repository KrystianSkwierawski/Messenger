using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.UnitTests.Common
{
    public static class ContextFactory
    {
        public static Context Create()
        {
            var options = new DbContextOptionsBuilder<Context>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                     .Options;

            var context = new Context(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static void Destroy(Context context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
