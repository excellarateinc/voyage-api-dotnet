using System.Net.Http;

namespace Launchpad.Web.IntegrationTests.Extensions
{
    /// <summary>
    /// Extension method that provides the Should() syntax for custom assertions
    /// </summary>
    public static class AssertionExtensions
    {

        public static HttpResponseMessageAssertions Should(this HttpResponseMessage actualValue)
        {
            return new HttpResponseMessageAssertions(actualValue);
        }
    }
}
