using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Launchpad.UnitTests.Common
{
    public static class ReflectionHelper
    {
        public static MethodInfo GetMethod<TType>(Expression<Action<TType>> expression)
        {
            var member = expression.Body as MethodCallExpression;

            if (member != null)
                return member.Method;

            throw new ArgumentException("Expression is not a method", "expression");
        }
    }
}
