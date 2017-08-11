using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Api.API.V1;
using Voyage.Api.UnitTests.Common;
using Voyage.Core.Exceptions;
using Voyage.Models;
using Voyage.Services.Admin;
using Xunit;

namespace Voyage.Api.UnitTests.API.V1
{
    public class AdminControllerTests : BaseUnitTest
    {
        private readonly AdminController _controller;
        private readonly Mock<IAdminService> _mockAdminService;
        public AdminControllerTests()
        {
            _mockAdminService = Mock.Create<IAdminService>();

            _controller = new AdminController(_mockAdminService.Object)
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
            var changeAccountStatusModel = Fixture.Create<ChangeAccountStatusModel>();
            var id = Fixture.Create<string>();

            _mockAdminService.Setup(_ => _.ToggleAccountStatus(id, changeAccountStatusModel))
                .ReturnsAsync(userModel);
            
            var result = await _controller.ToggleAccountStatus(id, changeAccountStatusModel);
            
            Mock.VerifyAll();
        }  

    }
}
