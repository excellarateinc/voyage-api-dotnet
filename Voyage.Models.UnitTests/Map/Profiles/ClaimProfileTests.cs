using System.Security.Claims;
using FluentAssertions;
using Voyage.Models.Entities;
using Voyage.Models.UnitTests.Common.AutoMapperFixture;
using Xunit;

namespace Voyage.Models.UnitTests.Map.Profiles
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class ClaimProfileTests
    {
        private readonly AutoMapperFixture _mappingFixture;

        public ClaimProfileTests(AutoMapperFixture mappingFixture)
        {
            _mappingFixture = mappingFixture;
        }

        [Fact]
        public void Claim_Should_Map_To_ClaimModel()
        {
            var claim = new Claim("type1", "value1");

            var claimModel = _mappingFixture.MapperInstance.Map<PermissionModel>(claim);

            claimModel.Should().NotBeNull();
            claimModel.PermissionType.Should().Be(claim.Type);
            claimModel.PermissionValue.Should().Be(claim.Value);
            claimModel.Id.Should().Be(default(int));
        }

        [Fact]
        public void RoleClaim_Should_Map_To_ClaimModel()
        {
            var roleClaim = new RolePermission
            {
                PermissionType = "type",
                PermissionValue = "value"
            };

            var claim = _mappingFixture.MapperInstance.Map<PermissionModel>(roleClaim);

            claim.Should().NotBeNull();
            claim.PermissionValue.Should().Be(roleClaim.PermissionValue);
            claim.PermissionType.Should().Be(roleClaim.PermissionType);
        }
    }
}
