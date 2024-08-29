# Use the official .NET SDK image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["eventify-api/eventify-api.csproj", "eventify-api/"]
RUN dotnet restore "eventify-api/eventify-api.csproj"

# Copy the entire project and build it
COPY . .
WORKDIR "/src/eventify-api"
RUN dotnet build "eventify-api.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "eventify-api.csproj" -c Release -o /app/publish

# Build the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eventify-api.dll"]
