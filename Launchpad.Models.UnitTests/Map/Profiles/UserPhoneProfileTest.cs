using Launchpad.Models.EntityFramework;
using Launchpad.Models.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Launchpad.Models.UnitTests.Map.Profiles
{
    [Trait("Category", "Mapping")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class UserPhonesProfileTest : BaseUnitTest
    {
        private AutoMapperFixture _mappingFixture;

        public UserPhonesProfileTest(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void UserPhoneEntity_Should_MapTo_UserPhoneModel()
        {
            var userPhone = new UserPhone();

            var model = _mappingFixture.MapperInstance.Map<UserPhoneModel>(userPhone);


        }
    }
}
