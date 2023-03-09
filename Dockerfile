# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

EXPOSE 80
EXPOSE 443

# copy csproj and restore as distinct layers
COPY *.sln .
COPY MoxControl/*.csproj ./MoxControl/
COPY MoxControl.Connect/*.csproj ./MoxControl.Connect/
COPY MoxControl.Connect.Proxmox/*.csproj ./MoxControl.Connect.Proxmox/
COPY MoxControl.Core/*.csproj ./MoxControl.Core/
COPY MoxControl.Data/*.csproj ./MoxControl.Data/
COPY MoxControl.Infrastructure/*.csproj ./MoxControl.Infrastructure/
COPY MoxControl.Models/*.csproj ./MoxControl.Models/
RUN dotnet restore

# copy everything else and build app
COPY MoxControl/. ./MoxControl/
COPY MoxControl.Connect/. ./MoxControl.Connect/
COPY MoxControl.Connect.Proxmox/. ./MoxControl.Connect.Proxmox/
COPY MoxControl.Core/. ./MoxControl.Core/
COPY MoxControl.Data/. ./MoxControl.Data/
COPY MoxControl.Infrastructure/. ./MoxControl.Infrastructure/
COPY MoxControl.Models/. ./MoxControl.Models/
WORKDIR /source/MoxControl
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MoxControl.dll"]