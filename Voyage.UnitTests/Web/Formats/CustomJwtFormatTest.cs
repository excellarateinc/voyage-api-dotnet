using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Owin.Security;
using Voyage.UnitTests.Common;
using Voyage.Web.Formats;
using Xunit;

namespace Voyage.UnitTests.Web.Formats
{
    public class CustomJwtFormatTest : BaseUnitTest
    {
        [Fact]
        public void JwtFormat_Should_Return_Correct_Payload_For_A_Given_Parameters()
        {
            // Setup JWT payload
            var jwtCustomFormat = new CustomJwtFormat();
            var authProps = new AuthenticationProperties
            {
                IssuedUtc = new DateTimeOffset(DateTime.Today),
                ExpiresUtc = new DateTimeOffset(DateTime.Today.AddDays(1))
            };
            var claims = new List<Claim>();
            var addUser = new Claim("user:addUser", "true");
            var manageUser = new Claim("user:manageUser", "false");
            claims.Add(addUser);
            claims.Add(manageUser);
            var claimIdentity = new ClaimsIdentity(claims);
            var ticket = new AuthenticationTicket(claimIdentity, authProps);

            // Generate JWT
            var token = jwtCustomFormat.Protect(ticket);

            // Assert
            Assert.True(HasCorrectClaim("iss", ConfigurationManager.AppSettings["oAuth:Issuer"], token));
            Assert.True(HasCorrectClaim("aud", ConfigurationManager.AppSettings["oAuth:Audience"], token));
            Assert.True(HasCorrectClaim("user:addUser", "true", token));
            Assert.True(HasCorrectClaim("user:manageUser", "false", token));
        }

        /// <summary>
        /// Check claim value against token claim
        /// </summary>
        /// <param name="claimName"></param>
        /// <param name="claimValue"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool HasCorrectClaim(string claimName, string claimValue, string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = ConfigurationManager.AppSettings["oAuth:Audience"],
                ValidIssuer = ConfigurationManager.AppSettings["oAuth:Issuer"],
                IssuerSigningKey = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["oAuth:SecretKey"]))
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var claimsPrincipal = jwtHandler.ValidateToken(token, validationParameters, out validatedToken);

            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimName);
            if (claim == null)
                return false;

            return claim.Value == claimValue;
        }
    }
}
