using System;
using FluentAssertions;
using Launchpad.UnitTests.Common;
using Xunit;
using Launchpad.Data.Interfaces;

namespace Launchpad.Data.UnitTests
{
    public class WidgetRepositoryTests : BaseUnitTest
    {
        public WidgetRepositoryTests()
        {
            Mock.Create<ILaunchpadDataContext>();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Context_Is_Null()
        {
            Action throwAction = () => new WidgetRepository(null);

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("context");
        }
    }
}
