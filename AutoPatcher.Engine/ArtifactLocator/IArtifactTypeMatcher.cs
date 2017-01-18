namespace AutoPatcher.Engine.ArtifactLocator
{
    public interface IArtifactTypeMatcher
    {
        bool IsOfType(string filePath);
    }
}
