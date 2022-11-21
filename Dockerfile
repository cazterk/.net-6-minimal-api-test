# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ChurchSystem.csproj .

RUN dotnet restore "ChurchSystem.csproj" 
COPY . .
RUN dotnet publish "ChurchSystem.csproj" -c release -o /app

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal  as final
WORKDIR /app
COPY --from=build /app .
EXPOSE 80

ENTRYPOINT ["dotnet", "ChurchSystem.dll"]

