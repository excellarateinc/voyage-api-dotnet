using System;
using System.Text;

namespace Voyage.Core
{
    public static class ExceptionExtensions
    {
        public static string FlattenMessages(this AggregateException exception)
        {
            const int maxDepth = 5;
            var depth = 0;

            var sb = new StringBuilder();

            Exception ex = exception;

            while (ex != null && depth < maxDepth)
            {
                sb.AppendLine(ex.Message);
                ex = ex.InnerException;
                ++depth;
            }

            return sb.ToString();
        }
    }
}
