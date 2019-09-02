# Info
This is a sample project showing you how to use protocol builder to create protocols for different target languages from its C# base.

# Pre Steps
## Using Releases
Run this to download the latest released version into 'tmp' folder:
```bash
sh UpdateToolsFromRelease.sh
```

## Using Source Code
- Edit UpdateToolsFromSource.sh and set protocol-builder source directory in the first line like:
```bash
SOURCEPATH=..
```
- Run the script to create and copy updated executable to local temp path:
```bash 
sh UpdateToolsFromSource.sh
```

# Steps
- Add/Edit C# files inside SampleProject folder (there are existing samples)
- Run GenerateAll script to generate defined protocol in target languages inside 'Output' folder