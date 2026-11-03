FROM ://microsoft.com AS build
WORKDIR /src
COPY . .
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish

FROM ://microsoft.com
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
