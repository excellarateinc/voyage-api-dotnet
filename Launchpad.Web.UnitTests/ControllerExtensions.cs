using Launchpad.UnitTests.Common;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using FluentAssertions;
using Launchpad.Web.Filters;

namespace Launchpad.Web.UnitTests
{
    public static class ControllerExtensions
    {
        public static void AssertClaim<TType>(this TType controller, 
            Expression<Action<TType>> expression, 
            string claimValue, 
            string claimType = Constants.LssClaims.Type) where TType : ApiController
        {
            ReflectionHelper.GetMethod(expression)
                .Should()
                .BeDecoratedWith<ClaimAuthorizeAttribute>(_ => _.ClaimValue == claimValue && _.ClaimType == claimType);
        }
    }
}
