using System;

namespace AutoPatcher.Engine.Util
{
    public static class Verify
    {
        public static void IsNotNullOrWhiteSpace(string str, string name)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("Null or whitespace parameter {0}", nameof(name));
            }
        }

        public static void IsNotNull<T>(T val, string name)
        {
            if (val == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
        }
    }
}
