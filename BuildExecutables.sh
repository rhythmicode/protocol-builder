cd ProtocolBuilderCli

dotnet publish -c release --runtime linux-x64 --output ../tmp/linux
dotnet publish -c release --runtime win-x64 --output ../tmp/windows

cd ..
curl -Lo warp-packer https://github.com/dgiagio/warp/releases/download/v0.3.0/linux-x64.warp-packer
chmod +x warp-packer
mkdir Executables
./warp-packer --arch linux-x64 --input_dir tmp/linux --exec pbuilder --output Executables/protocol-builder.sh
./warp-packer --arch windows-x64 --input_dir tmp/windows --exec pbuilder.exe --output Executables/protocol-builder.exe

rm -f warp-packer