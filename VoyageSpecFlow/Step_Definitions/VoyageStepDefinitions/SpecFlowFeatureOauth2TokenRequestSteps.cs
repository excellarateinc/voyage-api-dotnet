using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Text;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace VoyageSpecFlow.Step_Definitions.VoyageStepDefinitions
{

    [Binding]
    public class SpecFlowFeatureOauth2TokenRequestSteps
    {
        private const string BASIC = "Basic ";
        private const string APPLICATION_CONTENT_TYPE = "application/x-www-form-urlencoded";
        private const string APPLICATION_ACCEPT_JSON = "application/json";
        private static string generatedToken;
        private const string OAUTHTOKEN_URL = "http://localhost:8080/oauth/token";
        private const string TOKEN_NAME = "Voyage SUPER";
        private const string CLIENT_ID_VALUE = "client-super";
        private const string CLIENT_ID = "&client_id=";
        private const string CLIENT_SECRET_VALUE = "secret";
        private const string CLIENT_SECRET = "&client_secret=";
        private const string GRANT_TYPE_VALUE = "client_credentials";
        private const string GRANT_TYPE = "&grant_type=";
        private static string response;

        [Given(@"with token name '(.*)'")]
        public void GivenWithTokenName(string p0)
        {
            Assert.AreEqual(p0, TOKEN_NAME);
        }

        [Given(@"a Oauth(.*) url ""(.*)""")]
        public void GivenAOauthUrl(int p0, string p1)
        {
            Assert.AreEqual(p0, 2);
            Assert.AreEqual(p1, OAUTHTOKEN_URL);
        }

        [Given(@"with Client ID '(.*)'")]
        public void GivenWithClientID(string p0)
        {
            Assert.AreEqual(p0, CLIENT_ID_VALUE);
        }

        [Given(@"with Client Secret '(.*)'")]
        public void GivenWithClientSecret(string p0)
        {
            Assert.AreEqual(p0, CLIENT_SECRET_VALUE);
        }

        [Given(@"with Grant Type '(.*)'")]
        public void GivenWithGrantType(string p0)
        {
            Assert.AreEqual(p0, GRANT_TYPE_VALUE);
        }

        [When(@"I request the Oauth(.*) token form of this url")]
        public void WhenIRequestTheOauthTokenFormOfThisUrl(int p0)
        {
            try
            {
                WebClient client = new WebClient();
                string encodedCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(CLIENT_ID_VALUE + ":" + CLIENT_SECRET_VALUE));
                client.Headers[HttpRequestHeader.Authorization] = BASIC + encodedCredentials;
                client.Headers[HttpRequestHeader.Accept] = APPLICATION_ACCEPT_JSON;
                client.Headers[HttpRequestHeader.ContentType] = APPLICATION_CONTENT_TYPE;
                string postPayloadData = CLIENT_ID + CLIENT_ID_VALUE;
                postPayloadData += CLIENT_SECRET + CLIENT_SECRET_VALUE;
                postPayloadData += GRANT_TYPE + GRANT_TYPE_VALUE;
                //"{\"access_token\":\"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJjcmVhdGVkIjoxNTEwNTUzMDQ5ODk1LCJzY29wZSI6WyJSZWFkIERhdGEiLCJXcml0ZSBEYXRhIl0sImV4cCI6MTUxMDU2MDI0OSwiYXV0aG9yaXRpZXMiOlsiYXBpLnBlcm1pc3Npb25zLmRlbGV0ZSIsImFwaS5yb2xlcy5kZWxldGUiLCJhcGkucm9sZXMudXBkYXRlIiwiYXBpLnBlcm1pc3Npb25zLmxpc3QiLCJhcGkucGVybWlzc2lvbnMudXBkYXRlIiwiYXBpLnVzZXJzLmNyZWF0ZSIsImFwaS51c2Vycy5nZXQiLCJhcGkudXNlcnMubGlzdCIsImFwaS5wZXJtaXNzaW9ucy5nZXQiLCJhcGkucm9sZXMuZ2V0IiwiYXBpLnVzZXJzLnVwZGF0ZSIsImFwaS5yb2xlcy5jcmVhdGUiLCJhcGkudXNlcnMuZGVsZXRlIiwiYXBpLnBlcm1pc3Npb25zLmNyZWF0ZSIsImFwaS5yb2xlcy5saXN0Il0sImp0aSI6ImUzMTkxYjgyLWEwNzMtNDVkNC1hYTY1LTAyYjk4YTYyNzVhYSIsImNsaWVudF9pZCI6ImNsaWVudC1zdXBlciJ9.G7MP93f6cmdDwWrZRsOVEkSlzFoX0OZbVSZpoBAYi_o4S97nTb7IW7pmmrQEl-lbSOSJZ07drfr67hTNMug8zec_RknfFNpFwcgSloiOhbG5c8Abaa49WHnEC3-Piea8d_06U4C6sB5srbCfPLRWktEhkNWNp1zbofvkMQ64QZECkCv8Mti7zAvYTxMkubUz24Z_ZEG4Nb_yn-UWvrbgncT3FUGI8Opnj0GoczGyyQ__NiMyVRCv1AUMv_3xqUiEZs_Q3nw6N-Wz2asK19v-DbgaDg_qlutqxWoO50ZDEdhL3IYNkpQhEoxDxSd8ur7osNErsy1Yd0lAh1qatDjCnA\",\"token_type\":\"bearer\",\"expires_in\":7199,\"scope\":\"Read Data Write Data\",\"created\":1510553049895,\"jti\":\"e3191b82-a073-45d4-aa65-02b98a6275aa\"}"
                //"{\"access_token\":\"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJjcmVhdGVkIjoxNTEwNTUzMTU1MTE0LCJzY29wZSI6WyJSZWFkIERhdGEiLCJXcml0ZSBEYXRhIl0sImV4cCI6MTUxMDU2MDM1NSwiYXV0aG9yaXRpZXMiOlsiYXBpLnBlcm1pc3Npb25zLmRlbGV0ZSIsImFwaS5yb2xlcy5kZWxldGUiLCJhcGkucm9sZXMudXBkYXRlIiwiYXBpLnBlcm1pc3Npb25zLmxpc3QiLCJhcGkucGVybWlzc2lvbnMudXBkYXRlIiwiYXBpLnVzZXJzLmNyZWF0ZSIsImFwaS51c2Vycy5nZXQiLCJhcGkudXNlcnMubGlzdCIsImFwaS5wZXJtaXNzaW9ucy5nZXQiLCJhcGkucm9sZXMuZ2V0IiwiYXBpLnVzZXJzLnVwZGF0ZSIsImFwaS5yb2xlcy5jcmVhdGUiLCJhcGkudXNlcnMuZGVsZXRlIiwiYXBpLnBlcm1pc3Npb25zLmNyZWF0ZSIsImFwaS5yb2xlcy5saXN0Il0sImp0aSI6IjI2NDA5NDJjLTg5MmMtNDA5YS05Y2E4LTk1OWE1MzhjMjEyOCIsImNsaWVudF9pZCI6ImNsaWVudC1zdXBlciJ9.JA3wGqM_e4f2p9D8qD5bd3I9LmsdBCCLdGT8hIDPuF-AOIkt_yzkrWqfbbbsAy7vOYH0IkGVIpiRP8kwF5f_sHYC1g_z9P7WFrVF59S3dD8sJ43r24AvktoW4BLsxSQwOSHQunTf1mn8h_AckImWwecgpNh-wx74skMzZDNp8yT0FxYDf-lMp7rGRbh5qGvX7NHEJvfZv8r7qKiGfkiGzaVd7T6rFioVt6JCx6Pk62vCew44ZlSosevw1BlzdJuzRquqqTQXI8_Ao6-68ixVo8UF3YE59sMAZoakvgPiq8ohozR1-Ujqyr3-DX1p2Ex28fxfHjY9kFDvivjHL_s-PQ\",\"token_type\":\"bearer\",\"expires_in\":7199,\"scope\":\"Read Data Write Data\",\"created\":1510553155114,\"jti\":\"2640942c-892c-409a-9ca8-959a538c2128\"}"
                response = client.UploadString(OAUTHTOKEN_URL, postPayloadData);
                generatedToken = response.Substring(17, 1000);
                // TODO: ValidateJwtToken needs to be completed
                //var result1 = ValidateJwtToken(response);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Assert.True(generatedToken != null);
        }
        public static ClaimsPrincipal ValidateJwtToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler() { RequireExpirationTime = true };
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.Default.GetBytes(jwtToken));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(signingCredentials);

            var payload = new JwtPayload
            {
                {"access_token", response},
                {"scope", "Read Data Write Data"},
                //{ "created","1510553155114"},
                //{"jti","2640942c-892c-409a-9ca8-959a538c2128"},
            };

            var secToken = new JwtSecurityToken(header, payload);

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);

            var readToken = handler.ReadJwtToken(generatedToken);
            var rawHeader = readToken.RawHeader;

            var headers = Base64UrlEncoder.Decode(rawHeader);

            var myClaims = readToken.Claims;
            var claims = Base64UrlEncoder.Decode(myClaims.ToString());
            //var payload = header + "." + claims;

            //var signature = Base64UrlEncoder.Decode(HMACSHA256(payload, CLIENT_SECRET));

            //var encodedJWT = payload + "." + signature;

            return null;
        }


        [Then(@"I should obtain the following JSON message ""(.*)""")]
        public void ThenIShouldObtainTheFollowingJSONMessage(string p0)
        {
            Assert.NotNull(generatedToken);
        }

    }

}
