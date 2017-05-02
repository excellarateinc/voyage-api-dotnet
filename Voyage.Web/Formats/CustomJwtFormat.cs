using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Autofac;
using Microsoft.Owin.Security;
using Voyage.Services.KeyContainer;

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

            JwtPayload jwtPayLoad = new JwtPayload(issuer, audience, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime);

            // Sign key
            var rsaProvider = ContainerConfig.Container.Resolve<IRsaKeyContainerService>();

            // var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaProvider.GetRsaCryptoServiceProviderFromKeyContainer()), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);
            var key = rsaProvider.GetRsaCryptoServiceProviderFromKeyContainer();

            // Create jwt token
            // var token = new JwtSecurityToken(issuer, audience, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);
            string jwe = Jose.JWT.Encode(jwtPayLoad, key, Jose.JweAlgorithm.RSA_OAEP, Jose.JweEncryption.A256GCM);

            var claims = Jose.JWT.Decode(jwe, key);

            // Create jwt handler to generate token string
            // var handler = new JwtSecurityTokenHandler();
            // var jwt = handler.WriteToken(token);
            return jwe;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}