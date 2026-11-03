# Etapa base: runtime. Visual Studio la usa directamente al depurar (Fast mode).
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
# HTTP en 8080.
EXPOSE 8080

# Etapa build: restaura y compila con el SDK de .NET 10.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Se copia primero solo el .csproj para que la capa de "restore" quede en caché
# mientras no cambien los paquetes NuGet.
COPY ["personapi-dotnet/personapi-dotnet.csproj", "personapi-dotnet/"]
RUN dotnet restore "./personapi-dotnet/personapi-dotnet.csproj"
COPY . .
WORKDIR "/src/personapi-dotnet"
RUN dotnet build "./personapi-dotnet.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa publish: genera los binarios listos para producción.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./personapi-dotnet.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final: runtime + binarios publicados. Corre como usuario no root ($APP_UID).
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "personapi-dotnet.dll"]
