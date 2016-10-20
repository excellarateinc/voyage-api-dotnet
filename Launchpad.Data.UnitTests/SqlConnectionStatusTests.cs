using FluentAssertions;
using Launchpad.UnitTests.Common;
using System;
using Xunit;
using Ploeh.AutoFixture;

namespace Launchpad.Data.UnitTests
{
    public class SqlConnectionStatusTests : BaseUnitTest
    {
        [Fact]
        public void DisplayName_Should_Return_Initialized_Value()
        {
            var name = Fixture.Create<string>();
            var status = new SqlConnectionStatus("ABC", name);
            status.DisplayName.Should().Be(name);
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_ConnectionString_Is_Null()
        {
            Action throwAction = () => new SqlConnectionStatus(null, null);

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("connectionString");
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_DisplayName_Is_Null()
        {
            Action throwAction = () => new SqlConnectionStatus("ABC", null);

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("displayName");
        }
    }
}