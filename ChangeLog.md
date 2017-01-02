# 0.6.0.0 (2017-1-1):

### Enhancements
* Cleaned up a TON of cruft.
* Separated code into engine and UI to facilitate possible future Visual Studio extension.
* Added support for reverting patches. Now only supports "stock" and "patched revisions.
* Added support for setting root paths and adding files via relative paths, so configuration files are reusable on various machines.
* Moved remote binary root path out of the repository configuration and to user specific settings to facilitate reuse of repository configurations.
* Removed restriction requiring configuration files to be called "AutoPatch.xml"

# 0.5.0.0 (2016-12-28):

### Features
* Creating AutoPatch.xml files.
* Opening AutoPatch.xml files.
* Creating mappings from a local binary to a remote one.
* Creating mappings from set of many source files to a single local/remote binary pair (currently unused).
* Pressing "Patch Selected" renames the existing file and appends a time stamp, and copies in the new version.