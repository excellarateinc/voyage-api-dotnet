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
    [Trait("Cateogry", "Mapping")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class PhoneContactProfileTest : BaseUnitTest
    {
        private AutoMapperFixture _mappingFixture;

        public PhoneContactProfileTest(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void PhoneContactEntity_Should_MapTo_PhoneContactModel()
        {
            var userPhone = new UserPhone();

            var model = _mappingFixture.MapperInstance.Map<UserPhoneModel>(userPhone);


        }
    }
}
