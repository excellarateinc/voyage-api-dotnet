using System;

namespace Launchpad.Core
{
    public static class ArgumentExtensions
    {
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
