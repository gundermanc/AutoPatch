namespace AutoPatcher.Engine.ArtifactLocator
{
    public sealed class DllArtifactTypeMatcher : RegExArtifactTypeMatcherBase
    {
        private const string Pattern = @"^[a-z]:\\(.*)\.dll$";

        public DllArtifactTypeMatcher() : base(Pattern) { }
    }
}
