# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY PlayerService/PlayerService.csproj PlayerService/
RUN dotnet restore PlayerService/PlayerService.csproj

# Copy everything else and build
COPY PlayerService/ PlayerService/
WORKDIR /src/PlayerService
RUN dotnet build PlayerService.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish PlayerService.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy components folder
COPY PlayerService/components ./components

EXPOSE 5000
ENTRYPOINT ["dotnet", "PlayerService.dll"]