### Deprecated

This attempt at the problem focused too much on UX and not enough on the problems with patching, backups, and graceful recovery after updates.

A more robust patching tool can be found in my scripts repo https://github.com/gundermanc/tools

# AutoPatch
A lightweight tool for patching installed products with compiled bits

(C) 2014-2016 Christian Gunderman
Contact Email: gundermanc@gmail.com

## Introduction
AutoPatch is a lightweight tool for developers of large installed Windows applications with command line build systems. It is designed to make the individual developer more productive by speeding up the *compile*, *install specific build*, *patch installed product*, *perform repro steps*, *attach debugger*, *repeat* workflow. AutoPatch's primary goal is to facilitate the compile and patch stages of this workflow by automating compilation and error navigation, and by patching and reverting binary bits between machines on a network. A secondary goal is to do so with a minimum of prior configuration or user intervention, so, AutoPatch will eventually be able to guess and generate patch configuration files using a combination of Git repo state, file time stamps, build file parsing, and file access watching, so that teams don't have to maintain configuration files for it to be useful.

## Current User Scenario
- User is making an experimental change on bits that are compiled locally, but deployed to an installed product on a separate machine.
- User manually specifies mapping from local binary files to their remote locations (can be on the same machine).
- UI presents a list of build artifacts that can be multi-selected and patched in one click.
- If the user encounters a regressed behavior, the application can be returned to stock bits via the "Revert Patched Files" button.

The main drawback of the current user experience is the time required to configure the tool for first use. For users that are patching a small number of binaries, the tool may prove adequate. However, for users that are unfamilar with the code that they are patching, they must first determine which binary must be patched. These problems will be addressed in the "Ideal User Scenario" that I am work towards.

## Ideal User Scenario
- User is making an experimental change on bits that are compiled locally, but deployed to an installed product on a separate machine.
- User has freshly cloned the repo and has no existing AutoPatch configuration file.
- User simply launches AutoPatch, specifies a local binaries root, remote binaries root, and local sources path, and starts making changes.
- AutoPatch watches the Git repo status, and noticing the dirty files, offers the ability to compile all dirty assemblies.
- If the dirty file's project is unknown, AutoPatch will attempt to determine it using one of several project system plugins, designed to interpret MSBuild or other project files to find the source to assembly mapping.
- This mapping is then added to the configuration file automatically.
- User will be able to specify a rule set for scraping errors from command line output and showing it in an error list that can launch an editor to the specific file and line number.
- There will be UI cues and a button for selecting all assemblies that have associated "dirty" source files since the last commit.
- There will be UI cues and a button for selecting all assemblies that have been rebuilt since the last commit.
- User will be able to press a button to deploy all selected binaries.
- User will be able to press a button to build all selected binaries.
- User will be able to enable automatic build or deploy of binaries as soon as they are modified, without any user interaction.

## Getting Started With Current Functionality
- If you haven't already, you may need to download and install Microsoft .NET Framework v.4.5.
- Download a release ZIP and extract all files somewhere.
- Run AutoPatcher.exe
- Click *File* -> *New Repo*, and create a repo profile somewhere for your code repository.
- Click *Configuration* -> *Edit Binary Directories*.
- Set "Local Binaries Directory" to the root of your project's build output directory (e.g.: %userprofile%\Source\Repos\Project\bin\Debug).
- Set "Remote Binaries Directory" to the root of your installed project's directory (e.g.: %programfiles%\Company\Product).
- To deploy bits to a different computer or machine, you can use the [Windows Admin Share](https://en.wikipedia.org/wiki/Administrative_share) by typing "\\My-PC-Name\C$\Program Files\", for example, to access the C drive on any machine that you have admin access to with your current domain credentials. (Eventually there will be a UI workflow for this ;)
- To use the GUI file explorer, press the ". . ." button.
- Once you have specified both paths, press "Ok".
- Click *Configuration* -> *Edit Patch Scheme* to launch the patch scheme editor.
- The box on the left is a list of binaries to patch. Click "Add" to add a new binary, and in the dialog box, specify the path to the local and remote versions of the assembly, relative to the local binaries root and remote binaries root, respectively.
- Using the ". . ." buttons here is a lot easier than typing out the paths.
- Press "Ok".
- In the main window, select some files to patch with Shift + Click and press Patch Selected Files to copy them to their remote destinations.
- Each time you patch, only the original unmodified stock bits are saved. Clicking "Revert Selected Files" returns them to their stock state.

## Changes
See the [Change Log](https://github.com/gundermanc/AutoPatch/blob/master/ChangeLog.md).
