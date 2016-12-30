﻿using System;
using System.Linq.Expressions;
using System.Web.Http;

using FluentAssertions;

using Launchpad.UnitTests.Common;
using Launchpad.Web;
using Launchpad.Web.Filters;

namespace Launchpad.UnitTests.Web
{
    public static class ControllerExtensions
    {
        public static void AssertClaim<TType>(
            this TType controller, 
            Expression<Action<TType>> expression, 
            string claimValue, 
            string claimType = Constants.LssClaims.Type) where TType : ApiController
        {
            ReflectionHelper.GetMethod(expression)
                .Should()
                .BeDecoratedWith<ClaimAuthorizeAttribute>(_ => _.ClaimValue == claimValue && _.ClaimType == claimType);
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
