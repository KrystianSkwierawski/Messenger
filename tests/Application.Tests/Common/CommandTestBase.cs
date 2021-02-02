using Application.IntegrationTests.Common;
using Infrastructure.Persistence;
using System;

namespace Application.Tests.Common
{
    public class CommandTestBase : IDisposable
    {
        protected readonly Context _context;

        public CommandTestBase()
        {
            _context = ContextFactory.Create();
        }

        public void Dispose()
        {
            ContextFactory.Destroy(_context);
        }
    }
}
