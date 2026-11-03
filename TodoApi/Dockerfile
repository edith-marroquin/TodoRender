FROM ://microsoft.com AS build
WORKDIR /src

# Copy everything from the TodoApi folder
COPY . . 

# Because Render is already inside the TodoApi folder, 
# your .csproj file is right here in the current directory (.)
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish 

FROM ://microsoft.com
WORKDIR /app 
COPY --from=build /app/publish . 
ENTRYPOINT ["dotnet", "TodoApi.dll"]
