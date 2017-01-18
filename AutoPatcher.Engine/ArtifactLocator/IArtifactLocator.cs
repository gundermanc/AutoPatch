namespace AutoPatcher.Engine.ArtifactLocator
{
    public interface IArtifactLocator
    {
        string GetPathFromAssemblyName(string assemblyName);
    }
}
