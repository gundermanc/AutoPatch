using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoPatcher.Engine.Abstractions;
using AutoPatcher.Engine.Config;
using AutoPatcher.Engine.Properties;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine.Repository
{
    internal static class RepositoryLoader
    {
        public static IRepositoryInternal CreateEmptyFile(IErrorDialogs dialogs, string filePath)
        {
            Verify.IsNotNull(dialogs, nameof(dialogs));
            Verify.IsNotNullOrWhiteSpace(filePath, nameof(filePath));

            var repo = new Repository(filePath, null, null, Enumerable.Empty<BuildArtifact>());

            WriteToFile(dialogs, repo);

            return repo;
        }

        public static IRepositoryInternal CreateFromFile(IErrorDialogs dialogs, string filePath)
        {
            Verify.IsNotNull(dialogs, nameof(dialogs));
            Verify.IsNotNullOrWhiteSpace(filePath, nameof(filePath));

            Exception readException = null;

            try
            {
                var configurationData = RepositoryConfigurationDataLoader.OpenFromFile(filePath);

                return RepositoryLoader.RepositoryFromConfiguration(filePath, configurationData);
            }
            catch (Exception ex)
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

        public static void WriteToFile(IErrorDialogs dialogs, IRepository repository)
        {
            Verify.IsNotNull(dialogs, nameof(dialogs));
            Verify.IsNotNull(repository, nameof(repository));

            Exception writeException = null;

            try
            {
                var configData = RepositoryLoader.ConfigurationFromRepository(repository);

                RepositoryConfigurationDataLoader.WriteToFile(repository.RepositoryConfigurationFilePath, configData);
            }
            catch (Exception ex)
            {
                writeException = ex;
            }

            if (writeException != null)
            {
                dialogs.ErrorDialog(
                    string.Format(
                        Resources.StringConfigurationWriteFailure,
                        repository.RepositoryConfigurationFilePath,
                        writeException.Message));
            }
        }

        private static RepositoryConfigurationData ConfigurationFromRepository(IRepository repository)
        {
            return new RepositoryConfigurationData()
            {
                LocalBinRoot = repository.LocalBinRoot,
                SourceItemsRoot = repository.SourceItemsRoot,
                BuildArtifacts = new List<BuildArtifactData>(
                    repository.BuildArtifacts.Select(ba => new BuildArtifactData()
                    {
                        LocalPath = ba.LocalPath,
                        RemotePath = ba.RemotePath,
                        SourceItems = ba.SourceItems.Select(si => new SourceItemData() { LocalPath = si.LocalPath }).ToList()
                    }))
            };
        }

        private static IRepositoryInternal RepositoryFromConfiguration(string filePath, RepositoryConfigurationData config)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(filePath));
            Debug.Assert(config != null);

            return new Repository(
                filePath,
                config.LocalBinRoot,
                config.SourceItemsRoot,
                config.BuildArtifacts.Select(
                    ba => new BuildArtifact(
                        ba.LocalPath,
                        ba.RemotePath,
                        ba.SourceItems.Select(si => new SourceItem(si.LocalPath)))));
        }
    }
}
