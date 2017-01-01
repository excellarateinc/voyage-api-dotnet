using System;

namespace Launchpad.Core
{
    /// <summary>
    /// Extension methods for validating arguments
    /// </summary>
    public static class ArgumentExtensions
    {
        /// <summary>
        /// Throw null argument exception when the given object is null, otherwise returns the instance.
        /// </summary>
        /// <typeparam name="TType">Type of the argument</typeparam>
        /// <param name="argumentInstance">Instance to compare against null</param>
        /// <param name="paramName">Optional: Name of the argument as it appears in the caller's parameter list</param>
        /// <returns>TType</returns>
        public static TType ThrowIfNull<TType>(this TType argumentInstance, string paramName = "")
        {
            if (argumentInstance == null)
            {
                throw new ArgumentNullException(string.IsNullOrEmpty(paramName) ? typeof(TType).Name : paramName);
            }

            return argumentInstance;
        }

        /// <summary>
        /// Throw null argument exception when the given string is null or empty, otherwise returns the instance.
        /// </summary>
        /// <param name="argumentInstance">Instance to compare against null</param>
        /// <param name="paramName">Optional: Name of the argument as it appears in the caller's parameter list</param>
        /// <returns>string</returns>
        public static string ThrowIfNullOrEmpty(this string argumentInstance, string paramName = "")
        {
            if (string.IsNullOrEmpty(argumentInstance))
            {
                throw new ArgumentNullException(string.IsNullOrEmpty(paramName) ? typeof(string).Name : paramName);
            }

            return argumentInstance;
        }
    }
}
