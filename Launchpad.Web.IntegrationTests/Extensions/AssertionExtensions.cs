using System.Net.Http;

namespace Launchpad.Web.IntegrationTests.Extensions
{
    public static class AssertionExtensions
    {

        public static HttpResponseMessageAssertions Should(this HttpResponseMessage actualValue)
        {
            return new HttpResponseMessageAssertions(actualValue);
        }
    }
}
