using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AutoPatcher.Engine.Config
{
    [DataContract]
    internal sealed class BuildArtifactData
    {
        [DataMember]
        public string LocalPath { get; set; }

        [DataMember]
        public string RemotePath { get; set; }

        [DataMember]
        public List<SourceItemData> SourceItems { get; set; }
    }
}
