using System;
using System.IO;
using System.Runtime.Serialization;
using AutoPatcher.Abstractions;
using AutoPatcher.Properties;

namespace AutoPatcher.Config
{
    internal static class AppConfigurationLoader
    {
        public const string ConfigurationFileFilter = "AutoPatcher Configuration|AutoPatch.xml";
        public const string ConfigurationFileName = "AutoPatch.xml";

        public static AppConfiguration CreateAppConfigurationFromFile(IErrorDialogs dialogs, string filePath)
        {
            Exception readException = null;

            try
            {
                return AppConfiguration.OpenFromFile(filePath);
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
                dialogs.WarningDialog(
                    string.Format(
                        Resources.StringConfigurationLoadFailure,
                        filePath,
                        readException.Message));
            }

            return null;
        }

        public static void WriteAppConfiguration(IErrorDialogs dialogs, AppConfiguration appConfig)
        {
            Exception writeException = null;

            try
            {
                appConfig.WriteToFile();
            }
            catch (IOException ex)
            {
                writeException = ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                writeException = ex;
            }
            catch (SerializationException ex)
            {
                writeException = ex;
            }

            if (writeException != null)
            {
                dialogs.ErrorDialog(
                    string.Format(
                        Resources.StringConfigurationWriteFailure,
                        appConfig.FilePath,
                        writeException.Message));
            }
        }
    }
}
