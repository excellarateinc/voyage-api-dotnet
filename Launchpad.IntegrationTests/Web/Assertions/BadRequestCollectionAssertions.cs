using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using Launchpad.Models;

namespace Launchpad.IntegrationTests.Web.Assertions
{
    public class BadRequestCollectionAssertions :
       SelfReferencingCollectionAssertions<ResponseErrorModel, BadRequestCollectionAssertions>
    {
        public BadRequestCollectionAssertions(IEnumerable<ResponseErrorModel> actualValue) : base(actualValue)
        {
        }

        protected override string Context => "ResponseErrorModel";

        public AndConstraint<BadRequestCollectionAssertions> ContainErrorFor(string expectedDescription, string expectedCode, string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Any(error => error.Error == expectedCode && error.ErrorDescription == expectedDescription))
                .FailWith("Expected {context: response} to have error for {0} with code {1}{reason}, but it was not found.", expectedDescription, expectedCode);
            return new AndConstraint<BadRequestCollectionAssertions>(this);
        }
    }
}
