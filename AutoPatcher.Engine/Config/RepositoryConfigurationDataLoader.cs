using System.IO;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace AutoPatcher.Engine.Config
{
    internal static class RepositoryConfigurationDataLoader
    {
        private static DataContractSerializer serializer;

        private static DataContractSerializer Serializer
        {
            get
            {
                return serializer ?? (serializer = new DataContractSerializer(typeof(RepositoryConfigurationData)));
            }
        }

        public static RepositoryConfigurationData CreateEmpty()
        {
            return new RepositoryConfigurationData();
        }

        public static RepositoryConfigurationData OpenFromFile(string filePath)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(filePath));

            using (var file = File.OpenRead(filePath))
            {
                return Serializer.ReadObject(file) as RepositoryConfigurationData;
            }
        }

        public static void WriteToFile(string filePath, RepositoryConfigurationData configData)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(filePath));
            Debug.Assert(configData != null);

            using (var file = File.Open(filePath, FileMode.Create))
            {
                Serializer.WriteObject(file, configData);
            }
        }
    }
}
