SOURCEPATH=..
# Absolute path to this script, e.g. /home/user/bin/foo.sh
SCRIPT=$(readlink -f "$0")
# Absolute path this script is in, thus /home/user/bin
SCRIPTPATH=$(dirname "$SCRIPT")

cd $SOURCEPATH
sh BuildExecutables.sh
cd $SCRIPTPATH
mkdir -p tmp/tools/
cp ../tmp/protocol-builder tmp/tools/
chmod +X tmp/tools/protocol-builder
cp ../tmp/protocol-builder.exe tmp/tools/
