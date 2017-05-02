using Autofac;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Voyage.Services.KeyContainer;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace Voyage.Web.AuthProviders
{
    public class CustomTokenHandler : OAuthBearerAuthenticationProvider
    {
        public Task RequestToken(OAuthRequestTokenContext context)
        {
            string token = context.Token;
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.FromResult<object>(null);
        }

        // public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        // {
        //    var rsaProvider = ContainerConfig.Container.Resolve<IRsaKeyContainerService>();

        // var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaProvider.GetRsaCryptoServiceProviderFromKeyContainer()), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);
        //    var key = rsaProvider.GetRsaCryptoServiceProviderFromKeyContainer();
        //    var claimIndentity = new ClaimsIdentity();
        //    var claims = Jose.JWT.Decode(securityToken, key);
        //    validatedToken = CreateToken(new SecurityTokenDescriptor());
        //    return new ClaimsPrincipal();
        // }
    }
}