using System;
using System.Runtime.Serialization;

namespace AutoPatcher.Config
{
    [DataContract]
    internal sealed class SourceItemData : ICloneable
    {
        [DataMember]
        public string LocalPath { get; set; }

        public object Clone()
        {
            return new SourceItemData() { LocalPath = this.LocalPath };
        }
    }
}
