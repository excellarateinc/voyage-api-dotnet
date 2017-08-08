using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Api.API.V1;
using Voyage.Api.UnitTests.Common;
using Voyage.Core.Exceptions;
using Voyage.Models;
using Voyage.Services.User;
using Xunit;

namespace Voyage.Api.UnitTests.API.V1
{
    public class AdminControllerTests : BaseUnitTest
    {
        private readonly AdminController _controller;
        private readonly Mock<IUserService> _mockUserService;
        public AdminControllerTests()
        {
            _mockUserService = Mock.Create<IUserService>();

            _controller = new AdminController(_mockUserService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Fact]
        public void AdminController_Constructor_ParametersAreNull_ShouldThrowArgumentNullException()
        {
            Action throwAction = () => new AdminController(null);

            throwAction.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async void AdminController_ToggleAccountStatus_ShouldCallServiceToChangeUserStatus()
        {
            var userModel = Fixture.Create<UserModel>();
            var returnModel = Fixture.Create<UserModel>();
            var changeAccountStatusModel = Fixture.Create<ChangeAccountStatusModel>();
            var id = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.GetUserAsync(id)).ReturnsAsync(userModel);

            _mockUserService.Setup(_ => _.UpdateUserAsync(id, userModel))
                .ReturnsAsync(returnModel);
            
            var result = await _controller.ToggleAccountStatus(id, changeAccountStatusModel);
            
            Mock.VerifyAll();
        }

        [Fact]
        public void AdminController_ToggleAccountStatus_ShouldFailForEmptyStringId()
        {
            var changeAccountStatusModel = Fixture.Create<ChangeAccountStatusModel>();
            var id = "";
            
            Assert.ThrowsAsync<BadRequestException>(async () => await _controller.ToggleAccountStatus(id, changeAccountStatusModel));
        }

    }
}
