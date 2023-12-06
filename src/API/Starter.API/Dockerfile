#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/API/Starter.API/Starter.API.csproj", "src/API/Starter.API/"]
COPY ["src/Core/Starter.Application/Starter.Application.csproj", "src/Core/Starter.Application/"]
COPY ["src/Core/Starter.Domain/Starter.Domain.csproj", "src/Core/Starter.Domain/"]
COPY ["src/InfraStructure/Starter.Identity/Starter.Identity.csproj", "src/InfraStructure/Starter.Identity/"]
COPY ["src/InfraStructure/Starter.InfraStructure/Starter.InfraStructure.csproj", "src/InfraStructure/Starter.InfraStructure/"]
COPY ["src/InfraStructure/Starter.Persistence/Starter.Persistence.csproj", "src/InfraStructure/Starter.Persistence/"]
RUN dotnet restore "./src/API/Starter.API/./Starter.API.csproj"
COPY . .
WORKDIR "/src/src/API/Starter.API"
RUN dotnet build "./Starter.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Starter.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Starter.API.dll"]