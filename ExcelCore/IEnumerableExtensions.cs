using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelCore
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 查询位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static int FindIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(source));
            var index = 0;
            foreach (var item in source)
            {
                if (predicate(item)) return index;
                index++;
            }
            return -1;
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> doAction)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (doAction == null) throw new ArgumentNullException(nameof(doAction));
            ForEachInterator(source, doAction);
        }

        private static void ForEachInterator<TSource>(IEnumerable<TSource> source, Action<TSource, int> dpActoin)
        {
            var index = -1;
            foreach (TSource item in source)
            {
                index = checked(index + 1);
                dpActoin(item, index);
            }
        }
    }
}
