dotnet publish ../ProtocolBuilderCli -c Release
rsync -av --exclude UpdateToolsExcludedFilesList.txt ../ProtocolBuilderCli/bin/Release/netcoreapp2.2/publish/ ./Tools/ProtocolBuilder
