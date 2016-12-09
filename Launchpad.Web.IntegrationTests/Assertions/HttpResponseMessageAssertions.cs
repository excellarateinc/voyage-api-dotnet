using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Net;
using System.Net.Http;

namespace Launchpad.Web.IntegrationTests
{

    /// <summary>
    /// Custom assertions for HttpResponseMessage
    /// </summary>
    public class HttpResponseMessageAssertions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>
    {
        public HttpResponseMessageAssertions(HttpResponseMessage value)
        {
            Subject = value;
        }

        public AndConstraint<HttpResponseMessageAssertions> HaveHeader(string expectedHeader, string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Headers.Contains(expectedHeader))
                .FailWith("Expected {context: response} to have header {0}{reason}, but it was not found.", expectedHeader);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        public AndConstraint<HttpResponseMessageAssertions> HaveStatusCode(HttpStatusCode expectedStatusCode, string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(expectedStatusCode.Equals(Subject.StatusCode))
                .FailWith("Expected {context: status code} to be {0}{reason}, but found {1}.", expectedStatusCode, Subject.StatusCode);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        protected override string Context
        {
            get
            {
                return "HttpResponseMessage";
            }
        }
    }
}
