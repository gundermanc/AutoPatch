using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoPatcher.Engine.Util
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> items) where T : ICloneable
        {
            return items == null ? Enumerable.Empty<T>() : items.Select(i => i.Clone()).Cast<T>();
        }
    }
}