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
            if(argumentInstance == null)
            {
                throw new ArgumentNullException(String.IsNullOrEmpty(paramName) ? typeof(TType).Name : paramName);
            }
            return argumentInstance;
        }
    }
}
