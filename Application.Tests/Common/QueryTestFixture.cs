using Infrastructure.Persistence;
using System;
using Xunit;

namespace Application.IntegrationTests.Common
{
    public class QueryTestFixture : IDisposable
    {
        public Context Context { get; private set; }

        public QueryTestFixture()
        {
            Context = ContextFactory.Create();
        }

        public void Dispose()
        {
            ContextFactory.Destroy(Context);
        }

        [CollectionDefinition("QueryCollection")]
        public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
    }
}
