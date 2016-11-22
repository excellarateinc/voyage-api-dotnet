using Xunit;
using Launchpad.UnitTests.Common;
using Launchpad.Models.Validators;
using FluentValidation.TestHelper;

namespace Launchpad.Models.UnitTests.Validators
{

    public class RoleValidatorTests : BaseUnitTest
    {
        private RoleValidator _validator;

        public RoleValidatorTests()
        {
            _validator = new RoleValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(role => role.Name, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            _validator.ShouldHaveValidationErrorFor(role => role.Name, string.Empty);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_Populated()
        {
            _validator.ShouldNotHaveValidationErrorFor(role => role.Name, "Role Name");
        }
    }
}
