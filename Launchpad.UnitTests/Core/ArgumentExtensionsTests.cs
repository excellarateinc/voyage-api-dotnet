using System;

using FluentAssertions;

using Launchpad.Core;

using Xunit;

namespace Launchpad.UnitTests.Core
{
    public class ArgumentExtensionsTests
    {
        [Fact]
        public void ThrowIfNull_Should_Return_Object_When_Not_Null()
        {
            var input = new object();

            var output = input.ThrowIfNull();

            output.Should().BeSameAs(input);
        }

        [Fact]
        public void ThrowIfNull_Should_Throw_ArgumentNullException_When_Null()
        {
            object input = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action throwAction = () => input.ThrowIfNull();

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("Object");
        }

        [Fact]
        public void ThrowIfNull_Should_Throw_ArgumentNullException_When_Null_And_Return_nameOfString()
        {
            object input = null;
            const string name = "My Name";

            // ReSharper disable once ExpressionIsAlwaysNull
            Action throwAction = () => input.ThrowIfNull(name);

            throwAction.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be(name);
        }
    }
}
