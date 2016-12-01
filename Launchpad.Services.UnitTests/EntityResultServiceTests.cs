using FluentAssertions;
using Launchpad.Models;
using Launchpad.UnitTests.Common;
using Ploeh.AutoFixture;
using Xunit;

namespace Launchpad.Services.UnitTests
{
    public class EntityResultServiceTests : BaseUnitTest
    {
        /// <summary>
        /// Wrapped the abstract class in order to test the methods
        /// </summary>
        public class TestPassThrough : EntityResultService
        {
            public EntityResult InvokeNotFound(object id)
            {
                return base.NotFound(id);
            }

            public EntityResult<TModel> InvokeNotFound<TModel>(object id)
                 where TModel : class
            {
                return base.NotFound<TModel>(id);
            }

            public EntityResult InvokeSuccess()
            {
                return base.Success();
            }

            public EntityResult<TModel> InvokeSuccess<TModel>(TModel model)
                 where TModel : class
            {
                return base.Success<TModel>(model);
            }

        }


        private TestPassThrough _testPassThrough;

        public EntityResultServiceTests()
        {
            _testPassThrough = new TestPassThrough();
        }

        [Fact]
        public void Success_Should_Return_Result()
        {
            var result = _testPassThrough.InvokeSuccess();
            result.Succeeded.Should().BeTrue();
            result.IsEntityNotFound.Should().BeFalse();
            result.Errors.Should().HaveCount(0);
        }

        [Fact]
        public void SuccessTModel_Should_Return_Result()
        {
            var model = new object();
            var result = _testPassThrough.InvokeSuccess(model);
            result.Succeeded.Should().BeTrue();
            result.IsEntityNotFound.Should().BeFalse();
            result.Errors.Should().HaveCount(0);
            result.Model.Should().Be(model);
        }

        [Fact]
        public void NotFound_Should_Return_Result()
        {
            var id = Fixture.Create<string>();
            var result = _testPassThrough
                .InvokeNotFound(id);

            result.Succeeded.Should().BeFalse();
            result.IsEntityNotFound.Should().BeTrue();
            result.Errors
                .Should()
                .HaveCount(1)
                .And
                .HaveElementAt(0, "notfound.entity::Could not locate entity with ID " + id);

        }


        [Fact]
        public void NotFoundTModel_Should_Returnn_Result()
        {

            var id = Fixture.Create<string>();
            var result = _testPassThrough
                .InvokeNotFound<object>(id);

            result.Succeeded.Should().BeFalse();
            result.IsEntityNotFound.Should().BeTrue();
            result.Errors
                .Should()
                .HaveCount(1)
                .And
                .HaveElementAt(0, "notfound.entity::Could not locate entity with ID " + id);

        }
    }
}
