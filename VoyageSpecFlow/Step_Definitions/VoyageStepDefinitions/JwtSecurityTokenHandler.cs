using System;
using System.IdentityModel.Tokens.Jwt;

namespace VoyageSpecFlow.Step_Definitions.VoyageStepDefinitions
{
    internal class JwtSecurityTokenHandler
    {
        public JwtSecurityTokenHandler()
        {
        }

        public bool RequireExpirationTime { get; set; }

        internal JwtSecurityToken ReadToken(string jwtToken)
        {
            throw new NotImplementedException();
        }
    }
}