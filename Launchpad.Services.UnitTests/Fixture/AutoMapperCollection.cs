using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Services.Fixture
{
    [CollectionDefinition(AutoMapperCollection.CollectionName)]
    public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        public const string CollectionName = "AutoMapper Collection";
    }
}
