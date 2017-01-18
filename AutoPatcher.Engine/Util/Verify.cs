using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoPatcher.Engine.Util
{
    public static class Verify
    {
        public static void IsNotNullOrWhiteSpace(string str, string name)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("Null or whitespace parameter", name);
            }
        }

        public static void IsNotNull<T>(T val, string name)
        {
            if (val == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void IsNotEmpty<TItem>(IEnumerable<TItem> val, string name)
        {
            if (!val.Any())
            {
                throw new ArgumentException("Expected non empty collection", name);
            }
        }
    }
}
