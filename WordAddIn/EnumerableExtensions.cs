using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaudiaVedantaPublications
{
    public static partial class Extensions
    {
        public static IEnumerable<IEnumerable<T>> GroupConsecutive<T>(this IEnumerable<T> source, Func<T, T, bool> predicate)
        {
            while (source.Any())
            {
                var firstInGroup = source.First();
                var group = source.TakeWhile(i => predicate(firstInGroup, i)).ToArray();
                yield return group;
                source = source.Skip(group.Count());
            }
        }
    }
}
