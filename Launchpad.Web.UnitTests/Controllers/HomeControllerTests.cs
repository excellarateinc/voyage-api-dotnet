using FluentAssertions;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Controllers;
using System.Web.Mvc;
using Xunit;

namespace Launchpad.Web.UnitTests.Controllers
{
    public class HomeControllerTests : BaseUnitTest
    {
        private HomeController _homeController;

        public HomeControllerTests()
        {
            _homeController = new HomeController();
        }

        [Fact]
        public void Index_Should_Redirect_To_Docs()
        {
            var result = _homeController.Index();

            (result is RedirectResult).Should().BeTrue();

            var redirectResult = (RedirectResult)result;
            redirectResult.Url.Should().Be("~/docs/");
        }

    }
}
