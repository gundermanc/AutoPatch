using System.IO;
using System.Linq;
using AutoPatcher.Engine.Properties;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine.ArtifactLocator
{
    public sealed class FileEnumArtifactLocator : IArtifactLocator
    {
        private readonly string enumPath;
        private readonly IArtifactTypeMatcher[] matchers;

        public FileEnumArtifactLocator(string enumPath, params IArtifactTypeMatcher[] matchers)
        {
            Verify.IsNotNull(enumPath, nameof(enumPath));
            Verify.IsNotEmpty(matchers, nameof(matchers));

            this.enumPath = enumPath;
            this.matchers = matchers;
        }

        public string GetPathFromAssemblyName(string assemblyName)
        {
            Verify.IsNotNullOrWhiteSpace(assemblyName, nameof(assemblyName));

            foreach (var file in Directory.EnumerateFiles(this.enumPath, "*", SearchOption.AllDirectories))
            {
                if (file.Contains(assemblyName) &&
                    this.matchers.Any(m => m.IsOfType(file)))
                {
                    return file;
                }
            }

            throw new ArtifactLocatorException(
                string.Format(
                    Resources.StringArtifactLocatorArtifactNotFoundFailure,
                    assemblyName));
        }
    }
}
