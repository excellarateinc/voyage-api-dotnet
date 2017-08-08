
using Moq;
using FluentAssertions;
using System;
using Voyage.Services.Admin;
using Voyage.Services.UnitTests.Common.AutoMapperFixture;
using Voyage.Services.User;
using Xunit;
using Ploeh.AutoFixture;
using Voyage.Models;
using Voyage.Services.UnitTests.Common;
using Voyage.Core.Exceptions;
using System.Net;

namespace Voyage.Services.UnitTests
{
    [Trait("Category", "Admin.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class AdminServiceTests: BaseUnitTest
    {
        private readonly AdminService _service;
        private readonly Mock<IUserService> _mockUserService;
        public AdminServiceTests()
        {
            _mockUserService = new Mock<IUserService>();

            _service = new AdminService(_mockUserService.Object);
        }

        [Fact]
        public void AdminService_Constructor_ParametersAreNull_ShouldThrowArgumentNullException()
        {
            Action throwAction = () => new AdminService(null);

            throwAction.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async void AdminService_ToggleAccountStatus_ShouldCallServiceToChangeUserStatus()
        {
            var userModel = Fixture.Create<UserModel>();
            var returnModel = Fixture.Create<UserModel>();
            var changeAccountStatusModel = Fixture.Create<ChangeAccountStatusModel>();
            var id = Fixture.Create<string>();

            _mockUserService.Setup(_ => _.GetUserAsync(id)).ReturnsAsync(userModel);

            _mockUserService.Setup(_ => _.UpdateUserAsync(id, userModel))
                .ReturnsAsync(returnModel);

            var result = await _service.ToggleAccountStatus(id, changeAccountStatusModel);
            Mock.VerifyAll();
        }

        [Fact]
        public void AdminService_ToggleAccountStatus_ShouldFailForEmptyStringId()
        {
            var changeAccountStatusModel = Fixture.Create<ChangeAccountStatusModel>();
            var id = "";

            var exceptionObj = Assert.ThrowsAsync<BadRequestException>(async () => await _service.ToggleAccountStatus(id, changeAccountStatusModel));

            Assert.NotNull(exceptionObj.Result);
            Assert.NotEmpty(exceptionObj.Result.Message);
            Assert.Equal(exceptionObj.Result.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}
