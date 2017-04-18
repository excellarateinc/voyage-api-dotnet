using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Autofac;
using Microsoft.Owin.Security;
using Voyage.Web.AuthProviders;

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
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            // Sign key
            var rsaProvider = ContainerConfig.Container.Resolve<RsaKeyContainerProvider>();
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaProvider.GetRsaCryptoServiceProviderFromKeyContainer()), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);

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