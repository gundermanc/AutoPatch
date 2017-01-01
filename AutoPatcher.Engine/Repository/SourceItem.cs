using System;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine.Repository
{
    public sealed class SourceItem : ICloneable
    {
        public SourceItem(string localPath)
        {
            Verify.IsNotNullOrWhiteSpace(localPath, nameof(localPath));

            this.LocalPath = localPath;
        }

        public string LocalPath { get; }

        public object Clone()
        {
            return new SourceItem(this.LocalPath);
        }
    }
}
