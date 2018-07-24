using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Core
{
    public static class EnumerableExtensions
    {
        public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<IEnumerable<T1>>> func)
        {
            return (await Task.WhenAll(enumeration.Select(func))).SelectMany(s => s);
        }
    }
}
