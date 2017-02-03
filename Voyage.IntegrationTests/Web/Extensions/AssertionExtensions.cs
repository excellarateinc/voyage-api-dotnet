using System.Collections.Generic;
using System.Net.Http;

using Voyage.IntegrationTests.Web.Assertions;
using Voyage.Models;

namespace Voyage.IntegrationTests.Web.Extensions
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

        public static BadRequestCollectionAssertions Should(this IEnumerable<ResponseErrorModel> actualValue)
        {
            return new BadRequestCollectionAssertions(actualValue);
        }
    }
}
