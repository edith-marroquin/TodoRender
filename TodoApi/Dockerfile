ENV MY_URL="https://microsoft.com"
FROM MY_URL AS build
WORKDIR /src
COPY . .
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish

FROM MY_URL
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
