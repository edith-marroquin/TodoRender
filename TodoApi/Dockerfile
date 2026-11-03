# Step 1: Use the official Microsoft .NET 10 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# Copy and restore project dependencies (optimises Docker layer caching)
COPY *.sln .
COPY *.csproj ./
RUN dotnet restore

# Copy all remaining source files and compile release binaries
COPY . .
RUN dotnet publish -c Release -o /app

# Step 2: Use the lean ASP.NET 10 runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app .

# Render expects web traffic directed to port 10000 by default.
# This forces the internal Kestrel engine to bind cleanly to Render's infrastructure.
ENV ASPNETCORE_HTTP_PORTS=10000

ENTRYPOINT ["dotnet", "TodoApi.dll"]
