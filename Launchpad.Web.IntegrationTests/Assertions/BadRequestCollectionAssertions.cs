using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using Launchpad.Models;
using System.Collections.Generic;
using System.Linq;

namespace Launchpad.Web.IntegrationTests.Assertions
{
    public class BadRequestCollectionAssertions :
       SelfReferencingCollectionAssertions<BadRequestErrorModel, BadRequestCollectionAssertions>
    {
        public BadRequestCollectionAssertions(IEnumerable<BadRequestErrorModel> actualValue) : base(actualValue)
        {
        }

        protected override string Context => "BadRequestErrorModel";

        public AndConstraint<BadRequestCollectionAssertions> ContainErrorFor(string expectedField, string expectedCode, string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Any(error => error.Code == expectedCode && error.Field == expectedField))
                .FailWith("Expected {context: response} to have error for {0} with code {1}{reason}, but it was not found.", expectedField, expectedCode);
            return new AndConstraint<BadRequestCollectionAssertions>(this);
        }
    }
}
