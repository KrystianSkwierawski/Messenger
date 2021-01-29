using NUnit.Framework;
using System.Threading.Tasks;

namespace Messenger.Application.IntegrationTests
{
    using static Testing;
    public class TestBase
    {
        [SetUp]
        public async Task SetUp()
        {
            await ResetState();
        }
    }
}
