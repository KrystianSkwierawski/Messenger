using System;

namespace Application.UnitTests.Common
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
