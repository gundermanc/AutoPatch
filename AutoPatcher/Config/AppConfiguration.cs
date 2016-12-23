using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace AutoPatcher.Config
{
    internal sealed class AppConfiguration
    {
        private static DataContractSerializer serializer;

        private AppConfiguration() { }

        public ConfigurationData Configuration
        {
            get;
            private set;
        }

        private static DataContractSerializer Serializer
        {
            get
            {
                return serializer ?? (serializer = new DataContractSerializer(typeof(ConfigurationData)));
            }
        }

        public static AppConfiguration Create()
        {
            return new AppConfiguration()
            {
                Configuration = new ConfigurationData()
            };
        }

        public static AppConfiguration CreateFromFile(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            {
                var configData = Serializer.ReadObject(file) as ConfigurationData;

                if (configData != null)
                {
                    return new AppConfiguration()
                    {
                        Configuration = configData
                    };
                }
            }

            return null;
        }

        public void WriteToFile(string fileName)
        {
            Debug.Assert(this.Configuration != null);

            using (var file = File.OpenWrite(fileName))
            {
                Serializer.WriteObject(file, this.Configuration);
            }
        }
    }
}
