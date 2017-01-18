namespace AutoPatcher.Engine.ArtifactLocator
{
    public sealed class ExeArtifactTypeMatcher : RegExArtifactTypeMatcherBase
    {
        private const string Pattern = @"^[a-z]:\\(.*)\.exe$";

        public ExeArtifactTypeMatcher() : base(Pattern) { }
    }
}
