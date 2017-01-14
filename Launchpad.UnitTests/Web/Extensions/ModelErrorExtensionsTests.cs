using System.Linq;
using System.Web.Http.ModelBinding;
using FluentAssertions;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Extensions;
using Xunit;

namespace Launchpad.UnitTests.Web.Extensions
{
    public class ModelErrorExtensionsTests : BaseUnitTest
    {
        [Fact]
        public void ToModel_Should_Return_Model_Without_Code_If_Split_Fails()
        {
            const string field = "field1";
            const string error = "This is an error";
            var errorModel = new ModelError(error);

            var responseModel = errorModel.ToModel(field);

            responseModel.Error.Should().BeNull();
            responseModel.Field.Should().Be(field);
            responseModel.ErrorDescription.Should().Be(error);
        }

        [Fact]
        public void ToModel_Should_Return_Model_Without_Code_If_Split_Contains_Too_Many_Tokens()
        {
            const string field = "field1";
            const string error = "something::This is an error::another::split";
            var errorModel = new ModelError(error);

            var responseModel = errorModel.ToModel(field);

            responseModel.Error.Should().BeNull();
            responseModel.Field.Should().Be(field);
            responseModel.ErrorDescription.Should().Be(error);
        }

        [Fact]
        public void ToModel_Should_Return_Model_With_Code_If_Split_Succeeds()
        {
            const string field = "field1";
            const string error = "missing.field::This is an error";
            var errorModel = new ModelError(error);

            var responseModel = errorModel.ToModel(field);

            responseModel.Error.Should().Be("missing.field");
            responseModel.Field.Should().Be(field);
            responseModel.ErrorDescription.Should().Be("This is an error");
        }

        [Fact]
        public void ConvertToResponseModel_Should_Return_Enumerable_Of_Models()
        {
            var dictionary = new ModelStateDictionary();

            dictionary.AddModelError("field1", "Required field is missing");

            var result = dictionary.ConvertToResponseModel().ToList();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);

            var error = result.First();
            error.Field.Should().Be("field1");
            error.ErrorDescription.Should().Be("Required field is missing");
        }
    }
}
