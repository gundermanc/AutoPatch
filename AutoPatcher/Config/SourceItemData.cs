using System.Runtime.Serialization;

namespace AutoPatcher.Config
{
    [DataContract]
    internal sealed class SourceItemData
    {
        [DataMember]
        public string LocalPath { get; set; }
    }
}
