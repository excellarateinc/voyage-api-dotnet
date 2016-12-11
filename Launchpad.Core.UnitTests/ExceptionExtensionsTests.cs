using FluentAssertions;
using Launchpad.UnitTests.Common;
using System;
using Xunit;

namespace Launchpad.Core.UnitTests
{
    public class ExceptionExtensionsTests : BaseUnitTest
    {
        [Fact]
        public void FlattenMessages_Should_Stop_At_Depth_5()
        {
            var aggregateException = new AggregateException("1", new Exception("2", new Exception("3", new Exception("4", new Exception("5", new Exception("6"))))));
            var messages = aggregateException.FlattenMessages();
            messages.Should().NotContain("6");
        }

        [Fact]
        public void FlattenMessages_Should_Concatonate_Inner_Exception_Messages()
        {
            var aggregateException = new AggregateException("1", new Exception("2", new Exception("3")));
            var messages = aggregateException.FlattenMessages();
            messages.Should().Be("1\r\n2\r\n3\r\n");
        }
    }
}
