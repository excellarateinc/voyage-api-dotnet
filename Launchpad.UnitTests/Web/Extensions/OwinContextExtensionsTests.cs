using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

using FluentAssertions;

using Voyage.UnitTests.Common;
using Voyage.Web.Extensions;

using Microsoft.Owin;
using Microsoft.Owin.Security;

using Moq;

using Ploeh.AutoFixture;

using Xunit;

namespace Voyage.UnitTests.Web.Extensions
{
    [Trait("Category", "Extension Methods")]
    public class OwinContextExtensionsTests : BaseUnitTest
    {
        private readonly Mock<IOwinContext> _mockOwinContext;
        private readonly Mock<IOwinRequest> _mockOwinRequest;
        private readonly Mock<IOwinResponse> _mockOwinResponse;
        private readonly Mock<IAuthenticationManager> _mockAuthManager;
        private readonly Dictionary<string, object> _env;
        private readonly string _ipAddress;
        private readonly string _method;
        private readonly int _statusCode;
        private readonly string _path;
        private readonly string _overrideId;

        private void SetupAuditModelMocks()
        {
            _mockOwinContext.Setup(_ => _.Request).Returns(_mockOwinRequest.Object);
            _mockOwinContext.Setup(_ => _.Response).Returns(_mockOwinResponse.Object);
            _mockOwinContext.Setup(_ => _.Environment).Returns(_env);
            _mockOwinRequest.Setup(_ => _.RemoteIpAddress).Returns(_ipAddress);
            _mockOwinRequest.Setup(_ => _.Method).Returns(_method);
            _mockOwinRequest.Setup(_ => _.Path).Returns(new PathString(_path));
            _mockOwinResponse.Setup(_ => _.StatusCode).Returns(_statusCode);
        }

        public OwinContextExtensionsTests()
        {
            _mockOwinContext = Mock.Create<IOwinContext>();
            _mockOwinRequest = Mock.Create<IOwinRequest>();
            _mockOwinResponse = Mock.Create<IOwinResponse>();
            _mockAuthManager = Mock.Create<IAuthenticationManager>();
            _env = new Dictionary<string, object> { { "owin.RequestId", Guid.NewGuid().ToString() } };

            _ipAddress = Fixture.Create<string>();

            _method = Fixture.Create<string>();

            _path = "/" + Fixture.Create<string>();

            _statusCode = 3000;

            _overrideId = Guid.NewGuid().ToString();
        }

        [Fact]
        public void ToAuditModel_Uses_OverrideId_When_RequestId_Empty()
        {
            // Arrange
            SetupAuditModelMocks();
            _mockOwinContext.Setup(_ => _.Authentication).Returns((IAuthenticationManager)null);
            _env["owin.RequestId"] = Guid.Empty.ToString();

            // Act
            var result = _mockOwinContext.Object.ToAuditModel(_overrideId);

            // Assert
            result.RequestId.Should().Be(_overrideId);
        }

        [Fact]
        public void GetIdentityName_Should_Return_NoIdentity_When_Authentication_Null()
        {
            // Arrange
            _mockOwinContext.Setup(_ => _.Authentication).Returns((IAuthenticationManager)null);

            // Act
            var result = _mockOwinContext.Object.GetIdentityName();

            result.Should().Be("No Identity");
        }

        [Fact]
        public void GetIdentityName_Should_Return_NoIdentity_When_User_Null()
        {
            // Arrange
            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns((ClaimsPrincipal)null);

            // Act
            var result = _mockOwinContext.Object.GetIdentityName();

            result.Should().Be("No Identity");
        }

        [Fact]
        public void GetIdentityName_Should_Return_NoIdentity_When_Identity_Null()
        {
            // Arrange
            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns(new ClaimsPrincipal());

            // Act
            var result = _mockOwinContext.Object.GetIdentityName();

            result.Should().Be("No Identity");
        }

        [Fact]
        public void GetIdentityName_Should_Return_NoIdentity_When_Name_Null()
        {
            // Arrange
            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns(new ClaimsPrincipal(new GenericIdentity(string.Empty)));

            // Act
            var result = _mockOwinContext.Object.GetIdentityName();

            result.Should().Be("No Identity");
        }

        [Fact]
        public void GetIdentityName_Should_Return_Name_When_Populated()
        {
            // Arrange
            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns(new ClaimsPrincipal(new GenericIdentity("admin@admin.com")));

            // Act
            var result = _mockOwinContext.Object.GetIdentityName();

            result.Should().Be("admin@admin.com");
        }

        [Fact]
        public void ToAuditModel_Returns_Model_NoIdentity()
        {
            // Arrange
            SetupAuditModelMocks();

            _mockOwinContext.Setup(_ => _.Authentication).Returns((IAuthenticationManager)null);

            // Act
            var result = _mockOwinContext.Object.ToAuditModel(_overrideId);

            // Assert
            result.IpAddress.Should().Be(_ipAddress);
            result.Method.Should().Be(_method);
            result.Path.Should().Be(_path);
            result.RequestId.Should().Be(_env["owin.RequestId"].ToString());
            result.StatusCode.Should().Be(_statusCode);
            result.UserName.Should().Be("No Identity");
        }

        [Fact]
        public void ToAuditModel_Returns_Model_NoIdentity_Null_Principal()
        {
            // Arrange
            SetupAuditModelMocks();

            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns((ClaimsPrincipal)null);

            // Act
            var result = _mockOwinContext.Object.ToAuditModel(_overrideId);

            // Assert
            result.IpAddress.Should().Be(_ipAddress);
            result.Method.Should().Be(_method);
            result.Path.Should().Be(_path);
            result.RequestId.Should().Be(_env["owin.RequestId"].ToString());
            result.StatusCode.Should().Be(_statusCode);
            result.UserName.Should().Be("No Identity");
        }

        [Fact]
        public void ToAuditModel_Returns_Model_NoIdentity_Null_Identity()
        {
            // Arrange
            SetupAuditModelMocks();

            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns(new ClaimsPrincipal());

            // Act
            var result = _mockOwinContext.Object.ToAuditModel(_overrideId);

            // Assert
            result.IpAddress.Should().Be(_ipAddress);
            result.Method.Should().Be(_method);
            result.Path.Should().Be(_path);
            result.RequestId.Should().Be(_env["owin.RequestId"].ToString());
            result.StatusCode.Should().Be(_statusCode);
            result.UserName.Should().Be("No Identity");
        }

        [Fact]
        public void ToAuditModel_Returns_Model_UserName()
        {
            // Arrange
            SetupAuditModelMocks();
            var principal = new ClaimsPrincipal(new GenericIdentity("admin@admin.com"));

            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns(principal);

            // Act
            var result = _mockOwinContext.Object.ToAuditModel(_overrideId);

            // Assert
            result.IpAddress.Should().Be(_ipAddress);
            result.Method.Should().Be(_method);
            result.Path.Should().Be(_path);
            result.RequestId.Should().Be(_env["owin.RequestId"].ToString());
            result.StatusCode.Should().Be(_statusCode);
            result.UserName.Should().Be("admin@admin.com");
        }
    }
}
