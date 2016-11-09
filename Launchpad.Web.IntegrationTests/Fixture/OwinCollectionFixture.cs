using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Web.IntegrationTests.Fixture
{
    [CollectionDefinition(OwinCollectionFixture.Name)]
    public class OwinCollectionFixture : ICollectionFixture<OwinFixture>
    {
        public const string Name = "OwinCollectionFixture";
    }
}
