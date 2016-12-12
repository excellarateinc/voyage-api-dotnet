using FluentValidation.TestHelper;
using Launchpad.Models.Validators;
using Launchpad.UnitTests.Common;
using Xunit;

namespace Launchpad.Models.UnitTests.Validators
{
    [Trait("Category", "Model.Validation")]
    public class ClaimModelValidatorTests : BaseUnitTest
    {
        private readonly ClaimModelValidator _validator;

        public ClaimModelValidatorTests()
        {
            _validator = new ClaimModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_ClaimType_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(claim => claim.ClaimType, null as string);
        }

        [Fact]
        public void Should_Have_Error_When_ClaimValue_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(claim => claim.ClaimValue, null as string);
        }
    }
}
