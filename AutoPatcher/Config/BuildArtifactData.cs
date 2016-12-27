using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AutoPatcher.Util;

namespace AutoPatcher.Config
{
    [DataContract]
    internal sealed class BuildArtifactData : ICloneable
    {
        private List<SourceItemData> sourceItems;

        [DataMember]
        public string LocalPath { get; set; }

        [DataMember]
        public string RemotePath { get; set; }

        [DataMember]
        public List<SourceItemData> SourceItems
        {
            get
            {
                return this.sourceItems ?? (this.sourceItems = new List<SourceItemData>());
            }
        }

        public object Clone()
        {
            var data = new BuildArtifactData()
            {
                LocalPath = this.LocalPath,
                RemotePath = this.RemotePath
            };

            data.SourceItems.AddRange(this.SourceItems.Clone());

            return data;
        }
    }
}
