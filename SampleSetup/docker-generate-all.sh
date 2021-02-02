dotnet pbuilder.dll /source/ -o /output/Swift/ -l swift
dotnet pbuilder.dll /source/ -o /output/Kotlin/ -l kotlin
dotnet pbuilder.dll /source/ -o /output/TypeScript/ -l typescript --use-single-quotes-imports "true" --use-single-quotes-strings "true"
dotnet pbuilder.dll /source/ -o /output/PHP/ -l php --folder-hierarchy-skip-namespace-from-root 2 -n ""
dotnet pbuilder.dll /source/ -o /output/PHP_8/ -l php --folder-hierarchy-skip-namespace-from-root 2 --language-version "8.0" -n ""
