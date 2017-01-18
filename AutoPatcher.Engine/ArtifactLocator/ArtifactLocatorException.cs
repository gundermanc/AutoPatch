using System;

namespace AutoPatcher.Engine.ArtifactLocator
{
    internal sealed class ArtifactLocatorException : Exception
    {
        public ArtifactLocatorException(string message) : base(message) { }
    }
}
