FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ProtocolBuilderCli/ProtocolBuilderCli.csproj ProtocolBuilderCli/
COPY ProtocolBuilderLibCore/ProtocolBuilderLibCore.csproj ProtocolBuilderLibCore/
RUN dotnet restore ProtocolBuilderCli/ProtocolBuilderCli.csproj
RUN dotnet restore ProtocolBuilderLibCore/ProtocolBuilderLibCore.csproj

COPY . .
WORKDIR /src/ProtocolBuilderCli
RUN dotnet build ProtocolBuilderCli.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ProtocolBuilderCli.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "pbuilder.dll"]
