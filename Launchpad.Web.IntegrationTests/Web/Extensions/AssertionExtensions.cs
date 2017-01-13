using System.Collections.Generic;
using System.Net.Http;

using Launchpad.IntegrationTests.Web.Assertions;
using Launchpad.Models;

namespace Launchpad.IntegrationTests.Web.Extensions
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

        public static BadRequestCollectionAssertions Should(this IEnumerable<RequestErrorModel> actualValue)
        {
            return new BadRequestCollectionAssertions(actualValue);
        }
    }
}
