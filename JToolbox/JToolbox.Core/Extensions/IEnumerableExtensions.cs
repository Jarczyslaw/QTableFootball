using System;
using System.Collections.Generic;
using System.Linq;

namespace JToolbox.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int SafeMax<T>(this IEnumerable<T> enumerable, Func<T, int> selector, int defaultValue = default(int))
        {
            return enumerable.Any() ? enumerable.Max(selector) : defaultValue;
        }

        public static int SafeMin<T>(this IEnumerable<T> enumerable, Func<T, int> selector, int defaultValue = default(int))
        {
            return enumerable.Any() ? enumerable.Min(selector) : defaultValue;
        }

        public static List<T> SearchRecursively<T>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> childrenSelector, Predicate<T> predicate)
        {
            var result = new List<T>();
            @this.SearchRecursively(childrenSelector, predicate, result);
            return result;
        }

        public static void SearchRecursively<T>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> childrenSelector, Predicate<T> predicate, List<T> result)
        {
            if (@this != null)
            {
                foreach (var item in @this)
                {
                    if (predicate(item))
                    {
                        result.Add(item);
                    }

                    var children = childrenSelector(item);
                    children.SearchRecursively(childrenSelector, predicate, result);
                }
            }
        }
    }
}