# Info
This is a sample project showing you how to use protocol builder to create protocols for different target languages from its C# base.

# How To
We strongly recommend using the docker version, it's easier and more consistence :)
## Preparation
### Using Docker
You need to have docker installed (google it if you don't know how)
### Using Binary Releases
Run any of these depending on your OS to download the latest released version into 'tmp' folder:
Windows
```PowerShell
.\binary-prepare-win.ps1
```
Linux
```bash
sh ./binary-prepare-linux.sh
```
OSX (Mac)
```bash
sh ./binary-prepare-osx.sh
```

## Updating 'Protocol'
- Add/Edit C# files inside SampleProject folder (there are existing samples)

## Generate Outputs
### Using Docker
Run this to generate defined protocol in target languages inside 'Output' folder:
```docker-compose up```
### Using Binary Releases
Run any of these depending on your OS to generate defined protocol in target languages inside 'Output' folder:
Windows
```PowerShell
.\binary-generate-all-win.ps1
```
Linux
```bash
sh ./binary-generate-all-linux.sh
```
OSX (Mac)
```bash
sh ./binary-generate-all-osx.sh
```

# Customize for your project
## Using Docker
You need `docker-compose.yml` and `docker-generate-all.sh` in your project, edit `docker-generate-all.sh` depending on your project needs/structure.
## Using Binary Releases
You need all `binary-prepare-XXX.XXX` files and `binary-generate-all-XXX.XXX` files in your project, edit `binary-generate-all-XXX.XXX` files depending on your project needs/structure.