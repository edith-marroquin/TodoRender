ARG MY_URL1=://microsoft.com
FROM $MY_URL1 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish

ARG MY_URL2=://microsoft.com
ARG MY_URL2=://microsoft.com
FROM $MY_URL2
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
