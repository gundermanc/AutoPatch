using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AutoPatcher.Config
{
    [DataContract]
    internal sealed class ConfigurationData
    {
        private List<BuildArtifactData> buildArtifactData;

        [DataMember]
        public string LocalBinRoot { get; set; }

        [DataMember]
        public string RemoteBinRoot { get; set; }

        [DataMember]

        public string SourceItemsRoot { get; set; }

        [DataMember]
        public List<BuildArtifactData> BuildArtifacts
        {
            get
            {
                return this.buildArtifactData ?? (buildArtifactData = new List<BuildArtifactData>());
            }
        }
    }
}
