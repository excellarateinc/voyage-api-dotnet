using FluentAssertions;
using Launchpad.Web.AuthProviders;
using Xunit;

namespace Launchpad.Web.UnitTests.AuthProviders
{
    public class LoginOrchestratorV2Tests
    {
        private readonly LoginOrchestratorV2 _orchestrator;

        public LoginOrchestratorV2Tests()
        {
            _orchestrator = new LoginOrchestratorV2();
        }

        [Fact]
        public void TokenPath_Should_Be_V2()
        {
            _orchestrator.TokenPath.Should().Be("/api/v2/login");
        }
    }
}
