using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AutoPatcher.Config
{
    [DataContract]
    internal sealed class ConfigurationData
    {
        [DataMember]
        public List<BuildArtifactData> BuildArtifacts { get; }
    }
}
