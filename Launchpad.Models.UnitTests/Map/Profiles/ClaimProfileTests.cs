using Launchpad.Models.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using Xunit;
using FluentAssertions;
using Launchpad.Models.EntityFramework;
using System.Security.Claims;

namespace Launchpad.Models.UnitTests.Map.Profiles
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class ClaimProfileTests
    {
        private AutoMapperFixture _mappingFixture;

        public ClaimProfileTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void Claim_Should_Map_To_ClaimModel()
        {
            var claim = new Claim("type1", "value1");

            var claimModel = _mappingFixture.MapperInstance.Map<ClaimModel>(claim);

            claimModel.Should().NotBeNull();
            claimModel.ClaimType.Should().Be(claim.Type);
            claimModel.ClaimValue.Should().Be(claim.Value);
        }

        [Fact]
        public void RoleClaim_Should_Map_To_ClaimModel()
        {
            var roleClaim = new RoleClaim
            {
                ClaimType = "type",
                ClaimValue = "value"
            };

            var claim = _mappingFixture.MapperInstance.Map<ClaimModel>(roleClaim);

            claim.Should().NotBeNull();
            claim.ClaimValue.Should().Be(roleClaim.ClaimValue);
            claim.ClaimType.Should().Be(roleClaim.ClaimType);
        }

    }
}
