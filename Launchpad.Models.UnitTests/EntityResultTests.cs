using FluentAssertions;
using Launchpad.UnitTests.Common;
using Ploeh.AutoFixture;
using Xunit;

namespace Launchpad.Models.UnitTests
{
    public class EntityResultTests : BaseUnitTest
    {
        [Fact]
        public void Ctor_Should_Initialize_Flags()
        {
            var result = new EntityResult(true, true, "err1");
            result.IsEntityNotFound.Should().BeTrue();
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().HaveCount(1)
                .And
                .HaveElementAt(0, "err1");
        }

        [Fact]
        public void CtorTModel_Should_Initialize_Flags()
        {
            var model = new object();

            var result = new EntityResult<object>(model, true, true, "err1");
            result.IsEntityNotFound.Should().BeTrue();
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().HaveCount(1)
                .And
                .HaveElementAt(0, "err1");
            result.Model.Should().Be(model);
        }

        [Fact]
        public void WithErrorCodeMessage_Should_Format_Code_And_Message()
        {
            var result = new EntityResult(true, true)
                .WithErrorCodeMessage("code1", "err1");

            result.Errors
                .Should()
                .HaveCount(1)
                .And
                .HaveElementAt(0, "code1::err1");
        }

        [Fact]
        public void WithEntityNotFound_Should_Format_Message_With_Id()
        {
            var id = Fixture.Create<string>();
            var result = new EntityResult(true, true)
                .WithEntityNotFound(id);

            result.Errors
               .Should()
               .HaveCount(1)
               .And
               .HaveElementAt(0, "notfound.entity::Could not locate entity with ID " + id);
        }

        [Fact]
        public void WithErrorCodeMessageTModel_Should_Format_Code_And_Message()
        {
            var result = new EntityResult<object>(null, true, true)
                .WithErrorCodeMessage("code1", "err1");

            result.Errors
                .Should()
                .HaveCount(1)
                .And
                .HaveElementAt(0, "code1::err1");
        }

        [Fact]
        public void WithEntityNotFoundTModel_Should_Format_Message_With_Id()
        {
            var id = Fixture.Create<string>();
            var result = new EntityResult<object>(null, true, true)
                .WithEntityNotFound(id);

            result.Errors
               .Should()
               .HaveCount(1)
               .And
               .HaveElementAt(0, "notfound.entity::Could not locate entity with ID " + id);
        }
    }
}
