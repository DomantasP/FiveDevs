using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> items, int size)
        {
            if (!items.Any()) yield break;
            yield return items.Take(size);
            foreach (var chunk in items.Skip(size).Chunks(size))
            {
                yield return chunk;
            }
        }
    }
}
