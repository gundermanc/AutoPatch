using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AutoPatcher.Engine.Config
{
    [DataContract]
    internal sealed class RepositoryConfigurationData
    {
        [DataMember]
        public string LocalBinRoot { get; set; }

        [DataMember]
        public string SourceItemsRoot { get; set; }

        [DataMember]
        public List<BuildArtifactData> BuildArtifacts { get; set; }
    }
}
