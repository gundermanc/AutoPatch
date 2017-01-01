using System.Runtime.Serialization;

namespace AutoPatcher.Engine.Config
{
    [DataContract]
    internal sealed class SourceItemData
    {
        [DataMember]
        public string LocalPath { get; set; }
    }
}
