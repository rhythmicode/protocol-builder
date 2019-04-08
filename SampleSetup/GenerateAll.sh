cd "$(dirname "$0")"

dotnet ./Tools/ProtocolBuilder/pbuilder.dll ./Protocol/ -o ./Output/Swift/ -l swift
dotnet ./Tools/ProtocolBuilder/pbuilder.dll ./Protocol/ -o ./Output/Kotlin/ -l kotlin
dotnet ./Tools/ProtocolBuilder/pbuilder.dll ./Protocol/ -o ./Output/TypeScript/ -l typescript
