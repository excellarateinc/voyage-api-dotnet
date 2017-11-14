using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Text;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace VoyageSpecFlow
{
    [Binding]
    public class SpecFlowFeatureOauth2TokenRequestSteps
    {
        private const string BASIC = "Basic ";
        private const string BEARER = "Bearer ";
        private const string APPLICATION_CONTENT_TYPE = "application/x-www-form-urlencoded";
        private const string APPLICATION_ACCEPT_JSON = "application/json";
        private const string TOKEN_NAME = "Voyage SUPER";
        private string CLIENT_URL = "";
        private const string CLIENT_ID_VALUE = "client-super";
        private const string CLIENT_ID = "&client_id=";
        private const string CLIENT_SECRET_VALUE = "secret";
        private const string CLIENT_SECRET = "&client_secret=";
        private const string GRANT_TYPE_VALUE = "client_credentials";
        private const string GRANT_TYPE = "&grant_type=";
        private static string response;
        private string bearerToken;
        private string Unauthorized_Message = "The remote server returned an error: (401) Unauthorized.";
        private string FAILURE_MESSAGE;

        [Given(@"an access_token ""(.*)""")]
        public void GivenAnAccess_Token(string p0)
        {
            bearerToken = p0;
            Assert.NotNull(p0);
        }
        
        [Given(@"with ""(.*)""")]
        public void GivenWith(string p0)
        {
            CLIENT_URL = p0;
            Assert.NotNull(p0);
        }
        
        [When(@"I request the login through JWT token")]
        public void WhenIRequestTheLoginThroughJWTToken()
        {
            WebClient client = new WebClient();
            string encodedCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(CLIENT_ID_VALUE + ":" + CLIENT_SECRET_VALUE));
            //client.Headers[HttpRequestHeader.Authorization] = BASIC + encodedCredentials;
            client.Headers[HttpRequestHeader.Accept] = APPLICATION_ACCEPT_JSON;
            client.Headers[HttpRequestHeader.ContentType] = APPLICATION_CONTENT_TYPE;
            client.Headers[HttpRequestHeader.Authorization] = BEARER + bearerToken;
            string postPayloadData = CLIENT_ID + CLIENT_ID_VALUE;
            postPayloadData += CLIENT_SECRET + CLIENT_SECRET_VALUE;
            postPayloadData += GRANT_TYPE + GRANT_TYPE_VALUE;
            try
            {
                response = client.UploadString(CLIENT_URL, "");
            }
            catch (Exception e)
            {
                FAILURE_MESSAGE = e.Message;
                Assert.True(Unauthorized_Message.Equals(e.Message));
                return;
            }
            Assert.Fail();
        }
        [Then(@"I should get a failed loggin message ""(.*)""")]
        public void ThenIShouldGetAFailedLogginMessage(string p0)
        {
            Assert.True(Unauthorized_Message.Equals(p0));
        }

   }
}
