using System;
using FluentAssertions;
using Launchpad.UnitTests.Common;
using Xunit;
using Launchpad.Data.Interfaces;
using Moq;

namespace Launchpad.Data.UnitTests
{
    public class WidgetRepositoryTests : BaseUnitTest
    {
        private Mock<ILaunchpadDataContext> _mockContext;

        public WidgetRepositoryTests()
        {
            _mockContext = Mock.Create<ILaunchpadDataContext>();
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Context_Is_Null()
        {
            Action throwAction = () => new WidgetRepository(null);

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("context");
        }
    }
}
