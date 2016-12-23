using System;
using System.IO;
using System.Runtime.Serialization;
using AutoPatcher.Properties;

namespace AutoPatcher.Config
{
    internal static class AppConfigurationFactory
    {
        private const string ConfigurationFilePath = "config.xml";

        public static AppConfiguration CreateAppConfiguration(IErrorDialogs dialogs)
        {
            Exception readException = null;

            try
            {
                return AppConfiguration.CreateFromFile(ConfigurationFilePath);
            }
            catch (IOException ex)
            {
                readException = ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                readException = ex;
            }
            catch (SerializationException ex)
            {
                readException = ex;
            }

            if (readException != null)
            {
                CreateAppConfigurationFromTemplateIfApplicable(dialogs, readException);
            }

            return null;
        }

        private static AppConfiguration CreateAppConfigurationFromTemplateIfApplicable(IErrorDialogs dialogs, Exception readException)
        {
            var writeResult = WriteConfigurationTemplateIfApplicable();

            // Was there an exception writing the template?
            if (writeResult.Item2 == null)
            {
                // Did we try to write the template?
                if (writeResult.Item1)
                {
                    dialogs.WarningDialog(
                        string.Format(
                            Resources.StringConfigurationLoadFailureTemplateGenerated,
                            ConfigurationFilePath,
                            readException.Message));

                    return AppConfiguration.Create();
                }
                else
                {
                    dialogs.QueueExitAndErrorDialog(
                        string.Format(
                            Resources.StringConfigurationLoadFailure,
                            ConfigurationFilePath,
                            readException.Message));
                }
            }
            else
            {
                dialogs.QueueExitAndErrorDialog(
                    string.Format(
                        Resources.StringConfigurationLoadFailureTemplateWriteFailure,
                        ConfigurationFilePath,
                        readException.Message,
                        writeResult.Item2.Message));
            }

            return null;
        }

        private static Tuple<bool, Exception> WriteConfigurationTemplateIfApplicable()
        {
            bool hasWrittenTemplate = false;

            // Read config failed, try to create a config template and exit.
            var configTemplate = AppConfiguration.Create();

            try
            {
                if (!File.Exists(ConfigurationFilePath))
                {
                    configTemplate.WriteToFile(ConfigurationFilePath);
                    hasWrittenTemplate = true;
                }
            }
            catch (IOException ex)
            {
                return Tuple.Create<bool, Exception>(hasWrittenTemplate, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Tuple.Create<bool, Exception>(hasWrittenTemplate, ex);
            }
            catch (SerializationException ex)
            {
                return Tuple.Create<bool, Exception>(hasWrittenTemplate, ex);
            }

            return Tuple.Create<bool, Exception>(hasWrittenTemplate, null);
        }
    }
}
