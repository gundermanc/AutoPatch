using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// using System.Linq;
// using System.Xml.Linq;
using System.Xml.Linq;
using AutoPatcher.Engine.Abstractions;
using AutoPatcher.Engine.ArtifactLocator;
using AutoPatcher.Engine.Properties;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine.MSBuild
{
    public static class MSBuildArtifactImporter
    {
        public const string MSBuildFileFilter = "AutoPatcher Configuration|*.*proj";

        public static void Import(
            IErrorDialogs errorDialog,
            IState state,
            IArtifactLocator localBinLocator,
            IArtifactLocator remoteBinLocator,
            string fileName)
        {
            try
            {
                var doc = XElement.Load(fileName);

                var assemblyName = ScrapeAssemblyNameFromMSBuildProject(doc);
                var sourceItemsProjectRelativePaths = ScrapeRelativeFilePathsFromMSBuildProject(doc);
                var projectDirectoryRelativePath = PathUtil.PathRelativeToDirectory(
                    Path.GetDirectoryName(fileName),
                    state.Repository.SourceItemsRoot);

                var errorPathsList = new List<string>();
                var sourcePathsList = new List<string>();

                foreach (var filePath in sourceItemsProjectRelativePaths)
                {
                    var filePathRelativeToRepoRoot = Path.Combine(projectDirectoryRelativePath, filePath);

                    if (File.Exists(Path.Combine(state.Repository.SourceItemsRoot, filePathRelativeToRepoRoot)))
                    {
                        sourcePathsList.Add(filePathRelativeToRepoRoot);
                    }
                    else
                    {
                        errorPathsList.Add(filePathRelativeToRepoRoot);
                    }
                }

                var localArtifactPath = PathUtil.PathRelativeToDirectory(
                    localBinLocator.GetPathFromAssemblyName(assemblyName),
                    state.Repository.LocalBinRoot);
                var remoteArtifactPath = PathUtil.PathRelativeToDirectory(
                    remoteBinLocator.GetPathFromAssemblyName(assemblyName),
                    state.CurrentRemoteBinRoot);

                var buildArtifact = new BuildArtifact(
                    localArtifactPath,
                    remoteArtifactPath,
                    sourcePathsList.Select(i => new SourceItem(i)));

                state.AddBuildArtifactsRange(Enumerable.Repeat(buildArtifact, 1));
            }
            catch (Exception ex)
            {
                errorDialog.ErrorDialog(
                    string.Format(Resources.StringMSBuildScrapeFailure,
                    fileName,
                    ex.Message));
            }
        }

        private static string ScrapeAssemblyNameFromMSBuildProject(XElement doc)
        {
            var propertyGroupElements = doc.Elements()
                .Where(e => e.Name.LocalName == "PropertyGroup");

            var assemblyName = propertyGroupElements
                .First()
                .Elements()
                .First(e => e.Name.LocalName == "AssemblyName")
                .Value;

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new MSBuildImportException(Resources.StringMSBuildScrapeMissingAssemblyName);
            }

            return assemblyName;
        }

        private static IEnumerable<string> ScrapeRelativeFilePathsFromMSBuildProject(XElement doc)
        {
            var propertyGroupElements = doc.Elements()
                .Where(e => e.Name.LocalName == "PropertyGroup");

            var sourceFiles = new List<string>();

            var itemGroupElements = doc.Elements()
                .Where(e => e.Name.LocalName == "ItemGroup");

            foreach (var elem in itemGroupElements)
            {
                var compileElements = elem.Elements().Where(e => e.Name.LocalName == "Compile");
                var includeAttributes = compileElements.Attributes().Where(e => e.Name.LocalName == "Include");

                foreach (var includeValue in includeAttributes.Select(a => a.Value))
                {
                    yield return includeValue;
                }
            }
        }
    }
}
