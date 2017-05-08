using FluentAssertions;
using FluentValidation.Attributes;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Models.UnitTests
{
    public class ClaimModelTests
    {
        [Fact]
        public void Class_Should_Have_Validator_Class()
        {
            typeof(PermissionModel)
                .Should()
                .BeDecoratedWith<ValidatorAttribute>(_ => _.ValidatorType == typeof(PermissionModelValidator));
        }
    }
}
