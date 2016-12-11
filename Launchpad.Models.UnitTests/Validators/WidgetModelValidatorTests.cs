using FluentValidation.TestHelper;
using Launchpad.Models.Validators;
using Launchpad.UnitTests.Common;
using Xunit;

namespace Launchpad.Models.UnitTests.Validators
{
    [Trait("Category", "Model.Validation")]
    public class WidgetModelValidatorTests : BaseUnitTest
    {
        private readonly WidgetModelValidator _validator;

        public WidgetModelValidatorTests()
        {
            _validator = new WidgetModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Name, null as string);
        }
    }
}
