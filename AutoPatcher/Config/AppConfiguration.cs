using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace AutoPatcher.Config
{
    internal sealed class AppConfiguration
    {
        private static DataContractSerializer serializer;

        private AppConfiguration(string filePath)
        {
            this.FilePath = filePath;
        }

        public ConfigurationData Configuration
        {
            get;
            private set;
        }

        public string FilePath
        {
            get;
        }

        private static DataContractSerializer Serializer
        {
            get
            {
                return serializer ?? (serializer = new DataContractSerializer(typeof(ConfigurationData)));
            }
        }

        public static AppConfiguration Create(string filePath)
        {
            return new AppConfiguration(filePath)
            {
                Configuration = new ConfigurationData()
            };
        }

        public static AppConfiguration OpenFromFile(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                var configData = Serializer.ReadObject(file) as ConfigurationData;

                if (configData != null)
                {
                    return new AppConfiguration(filePath)
                    {
                        Configuration = configData
                    };
                }
            }

            return null;
        }

        public void WriteToFile()
        {
            Debug.Assert(this.Configuration != null);

            using (var file = File.Open(this.FilePath, FileMode.Create))
            {
                Serializer.WriteObject(file, this.Configuration);
            }
        }
    }
}
