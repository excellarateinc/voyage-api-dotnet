using Launchpad.Data.IntegrationTests.Extensions;
using Launchpad.Models.EntityFramework;
using System.Transactions;
using Xunit;
using FluentAssertions;

namespace Launchpad.Data.IntegrationTests
{
    [Collection(Constants.CollectionName)]
    public class RoleClaimRepositoryTests
    {
        [Fact]
        public void GetAll_Should_Return_RoleClaims()
        {
            using (new TransactionScope())
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    var repository = new RoleClaimRepository(ctx);

                    var role = ctx.AddRole();
                    ctx.AddRoleClaim(role);

                    var results = repository.GetAll();

                    results.Should().NotBeNullOrEmpty();
                }
            }
        }

        [Fact]
        public void GetByRoleAndClaim_Should_Return_Claim()
        {
            using (new TransactionScope())
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    var repository = new RoleClaimRepository(ctx);

                    var role = ctx.AddRole();
                    var claim = ctx.AddRoleClaim(role);

                    var fetchedClaim = repository.GetByRoleAndClaim(role.Name, claim.ClaimType, claim.ClaimValue);

                    fetchedClaim.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void GetClaimsByRole_Should_Return_Claims()
        {
            using (new TransactionScope())
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    var repository = new RoleClaimRepository(ctx);

                    var role = ctx.AddRole();
                    ctx.AddRoleClaim(role);

                    var claims = repository.GetClaimsByRole(role.Name);
                    claims.Should().NotBeNullOrEmpty();
                }
            }
        }

        [Fact]
        public void Get_Should_Return_RoleClaim()
        {
            using (new TransactionScope())
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    var repository = new RoleClaimRepository(ctx);

                    var role = ctx.AddRole();
                    var claim = ctx.AddRoleClaim(role);

                    var result = repository.Get(claim.Id);

                    result.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void Add_Should_Create_Claim()
        {
            using (new TransactionScope())
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    var repository = new RoleClaimRepository(ctx);

                    var role = ctx.AddRole();

                    var claim = new RoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = "CustomType1",
                        ClaimValue = "CustomValue1"
                    };

                    repository.Add(claim);
                    
                    var result = repository.Get(claim.Id);

                    result.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void Update_Should_Modify_Claim()
        {
            using (new TransactionScope())
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    var repository = new RoleClaimRepository(ctx);

                    var role = ctx.AddRole();

                    var claim = ctx.AddRoleClaim(role);

                    claim.ClaimValue = "Friday";

                    repository.Update(claim);

                    var result = repository.Get(claim.Id);

                    result.Should().NotBeNull();
                    result.ClaimValue.Should().Be("Friday");
                }
            }
        }

        [Fact]
        public void Delete_Should_Remove_Claim()
        {
            using (new TransactionScope())
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    var repository = new RoleClaimRepository(ctx);

                    var role = ctx.AddRole();

                    var claim = ctx.AddRoleClaim(role);
                   
                    repository.Delete(claim.Id);

                    var result = repository.Get(claim.Id);

                    result.Should().BeNull();
                }
            }
        }
    }
}

