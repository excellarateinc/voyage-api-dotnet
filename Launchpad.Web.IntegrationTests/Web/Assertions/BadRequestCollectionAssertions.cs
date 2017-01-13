using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using Launchpad.Models;

namespace Launchpad.IntegrationTests.Web.Assertions
{
    public class BadRequestCollectionAssertions :
       SelfReferencingCollectionAssertions<RequestErrorModel, BadRequestCollectionAssertions>
    {
        public BadRequestCollectionAssertions(IEnumerable<RequestErrorModel> actualValue) : base(actualValue)
        {
        }

        protected override string Context => "RequestErrorModel";

        public AndConstraint<BadRequestCollectionAssertions> ContainErrorFor(string expectedField, string expectedCode, string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Any(error => error.Error == expectedCode && error.Field == expectedField))
                .FailWith("Expected {context: response} to have error for {0} with code {1}{reason}, but it was not found.", expectedField, expectedCode);
            return new AndConstraint<BadRequestCollectionAssertions>(this);
        }
    }
}
