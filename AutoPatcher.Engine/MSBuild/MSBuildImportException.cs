using System;

namespace AutoPatcher.Engine.MSBuild
{
    internal sealed class MSBuildImportException : Exception
    {
        public MSBuildImportException(string message) : base(message) { }
    }
}
