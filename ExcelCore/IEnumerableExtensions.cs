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
    }
}
