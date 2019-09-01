# Absolute path to this script, e.g. /home/user/bin/foo.sh
SCRIPT=$(readlink -f "$0")
# Absolute path this script is in, thus /home/user/bin
SCRIPTPATH=$(dirname "$SCRIPT")
cd ..
sh BuildExecutables.sh
cd $SCRIPTPATH
cp ../tmp/protocol-builder ./Tools/
chmod +X ./Tools/protocol-builder
cp ../tmp/protocol-builder.exe ./Tools/
