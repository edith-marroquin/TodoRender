FROM mcr.microsoft.com/windows/servercore:ltsc2019 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/windows/servercore:ltsc2019
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
