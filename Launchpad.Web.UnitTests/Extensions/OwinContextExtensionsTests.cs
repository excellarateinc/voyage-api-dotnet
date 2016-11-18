using System;
using System.Collections.Generic;
using Xunit;
using Ploeh.AutoFixture;
using FluentAssertions;
using Launchpad.UnitTests.Common;
using Moq;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Launchpad.Web.Extensions;
using System.Security.Claims;
using System.Security.Principal;

namespace Launchpad.Web.UnitTests.Extensions
{
    [Trait("Category", "Extension Methods")]
    public class OwinContextExtensionsTests : BaseUnitTest
    {
        Mock<IOwinContext> _mockOwinContext;
        Mock<IOwinRequest> _mockOwinRequest;
        Mock<IOwinResponse> _mockOwinResponse;
        Mock<IAuthenticationManager> _mockAuthManager;
        Dictionary<string, object> _env;

        private string _ipAddress;
        private string _method;
        private int _statusCode;
        private string _path;


        public OwinContextExtensionsTests()
        {
            _mockOwinContext = Mock.Create<IOwinContext>();
            _mockOwinRequest = Mock.Create<IOwinRequest>();
            _mockOwinResponse = Mock.Create<IOwinResponse>();
            _mockAuthManager = Mock.Create<IAuthenticationManager>();
            _env = new Dictionary<string, object>();
            _env.Add("owin.RequestId", Guid.NewGuid().ToString());

            _mockOwinContext.Setup(_ => _.Request).Returns(_mockOwinRequest.Object);
            _mockOwinContext.Setup(_ => _.Response).Returns(_mockOwinResponse.Object);
            _mockOwinContext.Setup(_ => _.Environment).Returns(_env);

            _ipAddress = Fixture.Create<string>();
            _mockOwinRequest.Setup(_ => _.RemoteIpAddress).Returns(_ipAddress);
            _method = Fixture.Create<string>();
            _mockOwinRequest.Setup(_ => _.Method).Returns(_method);
            _path = "/" + Fixture.Create<string>();
            _mockOwinRequest.Setup(_ => _.Path).Returns(new PathString(_path));
            _statusCode = 3000;
            _mockOwinResponse.Setup(_ => _.StatusCode).Returns(_statusCode);

        }



        [Fact]
        public void ToAuditModel_Returns_Model_NoIdentity()
        {
           

            _mockOwinContext.Setup(_ => _.Authentication).Returns((IAuthenticationManager)null);

            //Act
            var result = _mockOwinContext.Object.ToAuditModel();

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


            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns((ClaimsPrincipal)null);

            //Act
            var result = _mockOwinContext.Object.ToAuditModel();

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


            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns(new ClaimsPrincipal());

            //Act
            var result = _mockOwinContext.Object.ToAuditModel();

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
            var principal = new ClaimsPrincipal((new GenericIdentity("admin@admin.com")));

            _mockOwinContext.Setup(_ => _.Authentication).Returns(_mockAuthManager.Object);
            _mockAuthManager.Setup(_ => _.User).Returns(principal);

            //Act
            var result = _mockOwinContext.Object.ToAuditModel();

            result.IpAddress.Should().Be(_ipAddress);
            result.Method.Should().Be(_method);
            result.Path.Should().Be(_path);
            result.RequestId.Should().Be(_env["owin.RequestId"].ToString());
            result.StatusCode.Should().Be(_statusCode);
            result.UserName.Should().Be("admin@admin.com");

        }
    }

}
