using System;
using System.Linq.Expressions;
using System.Web.Http;
using FluentAssertions;
using Voyage.Web.Filters;
using Voyage.Web.UnitTests.Common;

namespace Voyage.Web.UnitTests
{
    public static class ControllerExtensions
    {
        public static void AssertClaim<TType>(
            this TType controller,
            Expression<Action<TType>> expression,
            string claimValue,
            string claimType = Constants.AppPermissions.Type) where TType : ApiController
        {
            ReflectionHelper.GetMethod(expression)
                .Should()
                .BeDecoratedWith<PermissionAuthorizeAttribute>(_ => _.PermissionValue == claimValue && _.PermissionType == claimType);
        }

        public static void AssertRoute<TType>(
            this TType controller,
         Expression<Action<TType>> expression,
         string template) where TType : ApiController
        {
            ReflectionHelper.GetMethod(expression)
                .Should()
                .BeDecoratedWith<RouteAttribute>(_ => template.Equals(_.Template));
        }

        public static void AssertAttribute<TType, TAttribute>(
            this TType controller,
            Expression<Action<TType>> expression)
            where TType : ApiController
            where TAttribute : Attribute
        {
            ReflectionHelper.GetMethod(expression)
               .Should()
               .BeDecoratedWith<TAttribute>();
        }
    }
}
