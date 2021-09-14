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

        public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T> comparer = null)
        {
            var cnt = comparer == null
                ? new Dictionary<T, int>() : new Dictionary<T, int>(comparer);

            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }

            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        public static List<T> SearchRecursively<T>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> childrenSelector, Predicate<T> predicate, List<T> result = null)
        {
            result = result ?? new List<T>();

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
            return result;
        }
    }
}