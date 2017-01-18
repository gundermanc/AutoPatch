using System;
using AutoPatcher.Engine.Properties;

namespace AutoPatcher.Engine.Util
{
    public static class PathUtil
    {
        public static string PathRelativeToDirectory(string fullPath, string relativeTo)
        {
            if (!fullPath.StartsWith(relativeTo))
            {
                throw new InvalidOperationException(Resources.StringPathInputPathMustBeRelative);
            }

            return fullPath.Substring(relativeTo.Length).TrimStart('\\');
        }
    }
}
