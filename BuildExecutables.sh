cd ProtocolBuilderCli

dotnet publish -c release --runtime linux-x64 
dotnet publish -c release --runtime win-x64  

cd ..
curl -Lo warp-packer https://github.com/dgiagio/warp/releases/download/v0.3.0/linux-x64.warp-packer
chmod +x warp-packer
mkdir Executables
./warp-packer --arch linux-x64 --input_dir ProtocolBuilderCli/bin/release/netcoreapp2.2/linux-x64/publish --exec pbuilder --output Executables/protocol-builder.sh
./warp-packer --arch windows-x64 --input_dir ProtocolBuilderCli/bin/release/netcoreapp2.2/win-x64/publish --exec pbuilder.exe --output Executables/protocol-builder.exe

rm -rf ProtocolBuilderCli/bin
rm -f warp-packer