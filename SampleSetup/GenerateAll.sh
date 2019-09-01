cd "$(dirname "$0")"

./Tools/protocol-builder ./Protocol/ -o ./Output/Swift/ -l swift
./Tools/protocol-builder ./Protocol/ -o ./Output/Kotlin/ -l kotlin
./Tools/protocol-builder ./Protocol/ -o ./Output/TypeScript/ -l typescript
./Tools/protocol-builder ./Protocol/ -o ./Output/PHP/ -l php -n ""
