using Xunit;
using Launchpad.Models.Map.Resolvers;
using Launchpad.Models.Enum;
using Launchpad.Models.UnitTests.Fixture;
using FluentAssertions;

namespace Launchpad.Models.UnitTests.Map.Resolvers
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class LogLevelResolverTests 
    {
        private AutoMapperFixture _fixture;

        public LogLevelResolverTests(AutoMapperFixture fixture)
        {
            _fixture = fixture;
        }


        [Theory()]
        [InlineData("Information", StatusCode.OK)]
        [InlineData("Debug", StatusCode.OK)]
        [InlineData("Verbose", StatusCode.OK)]
        [InlineData("ABC", StatusCode.Critical)]
        [InlineData("Error", StatusCode.Critical)]
        public void Resolver_Should_Return_Expected_StatusCode(string level, StatusCode code)
        {
            var resolver = new LogLevelResolver();

            var result = resolver.Resolve(new object(), new object(), level, StatusCode.OK, null);

            result.Should().Be(code);

        }
    }
}
