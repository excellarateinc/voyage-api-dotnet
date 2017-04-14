using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Text;
using Microsoft.Owin.Security;

namespace Voyage.Web.Formats
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            /*
             * JWT Payload
             */
            var audience = ConfigurationManager.AppSettings["oAuth:Audience"];
            var issuer = ConfigurationManager.AppSettings["oAuth:Issuer"];
            var secretKey = ConfigurationManager.AppSettings["oAuth:SecretKey"];
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            // Sign key
            var signingKey = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Create credential base on given key and security algorithm
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            // Create jwt token
            var token = new JwtSecurityToken(issuer, audience, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);

            // Create jwt handler to generate token string
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}