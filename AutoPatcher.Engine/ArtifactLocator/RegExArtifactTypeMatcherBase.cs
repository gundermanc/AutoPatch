using System.Text.RegularExpressions;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine.ArtifactLocator
{
    public abstract class RegExArtifactTypeMatcherBase : IArtifactTypeMatcher
    {
        private readonly Regex regex;

        public RegExArtifactTypeMatcherBase(string pattern)
        {
            Verify.IsNotNullOrWhiteSpace(pattern, nameof(pattern));

            this.regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool IsOfType(string filePath)
        {
            Verify.IsNotNullOrWhiteSpace(filePath, nameof(filePath));

            return this.regex.IsMatch(filePath);
        }
    }
}
