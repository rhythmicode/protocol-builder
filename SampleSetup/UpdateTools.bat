dotnet publish ..\ProtocolBuilderCli -c Release
xcopy /exclude:UpdateToolsExcludedFilesList.txt ..\ProtocolBuilderCli\bin\Release\netcoreapp2.2\publish .\Tools\ProtocolBuilder /s /Y
xcopy /exclude:UpdateToolsExcludedFilesList.txt ..\ProtocolBuilderCli\bin\Release\netcoreapp2.2\publish .\Tools\ProtocolBuilder /s /Y
pause