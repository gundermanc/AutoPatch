using System.Runtime.Serialization;

namespace AutoPatcher.Config
{
    [DataContract]
    internal sealed class ConfigurationData
    {
        [DataMember]
        public string RepositoryRoot { get; set; }
    }
}
