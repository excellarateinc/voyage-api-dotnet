using FluentAssertions;
using FluentValidation.Attributes;
using Voyage.Models;
using Voyage.Models.Validators;
using Xunit;

namespace Voyage.Web.UnitTests.Models
{
    public class ClaimModelTests
    {
        [Fact]
        public void Class_Should_Have_Validator_Class()
        {
            typeof(ClaimModel)
                .Should()
                .BeDecoratedWith<ValidatorAttribute>(_ => _.ValidatorType == typeof(ClaimModelValidator));
        }
    }
}
