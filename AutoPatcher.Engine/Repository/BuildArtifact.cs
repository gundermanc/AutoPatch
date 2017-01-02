using System;
using System.Collections.Generic;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine.Repository
{
    public sealed class BuildArtifact : ICloneable
    {
        public BuildArtifact(string localPath, string remotePath, IEnumerable<SourceItem> sourceItems)
        {
            Verify.IsNotNullOrWhiteSpace(localPath, nameof(localPath));
            Verify.IsNotNullOrWhiteSpace(remotePath, nameof(remotePath));
            Verify.IsNotNull(sourceItems, nameof(sourceItems));

            this.IsPatched = false;
            this.LocalPath = localPath;
            this.RemotePath = remotePath;
            this.SourceItems = new HashSet<SourceItem>(sourceItems);
        }

        public bool IsPatched { get; set; }

        public string LocalPath { get; }

        public string RemotePath { get; }

        public ISet<SourceItem> SourceItems { get; }

        public object Clone()
        {
            return new BuildArtifact(this.LocalPath, this.RemotePath, this.SourceItems.Clone());
        }
    }
}
