using FluentValidation.TestHelper;
using Voyage.Models.UnitTests.Common;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Models.UnitTests.Validators
{
    [Trait("Category", "Model.Validation")]
    public class ClaimModelValidatorTests : BaseUnitTest
    {
        private readonly PermissionModelValidator _validator;

        public ClaimModelValidatorTests()
        {
            _validator = new PermissionModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_PermissionType_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(claim => claim.PermissionType, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_PermissionValue_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(claim => claim.PermissionValue, null as string);
        }
    }
}
