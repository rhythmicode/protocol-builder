cd ProtocolBuilderCli

dotnet publish -c release --runtime linux-x64 --output ../tmp/linux-publish
dotnet publish -c release --runtime osx-x64 --output ../tmp/osx-publish
dotnet publish -c release --runtime win-x64 --output ../tmp/windows-publish

cd ..
cd tmp
curl -Lo warp-packer https://github.com/dgiagio/warp/releases/download/v0.3.0/linux-x64.warp-packer
chmod +x warp-packer
./warp-packer --arch linux-x64 --input_dir linux-publish --exec pbuilder --output protocol-builder-linux
./warp-packer --arch macos-x64 --input_dir osx-publish --exec pbuilder --output protocol-builder-osx
./warp-packer --arch windows-x64 --input_dir windows-publish --exec pbuilder.exe --output protocol-builder.exe

cd ..