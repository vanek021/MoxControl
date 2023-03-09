# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY MoxControl/*.csproj ./MoxControl/
RUN dotnet restore

# copy everything else and build app
COPY MoxControl/. ./MoxControl/
WORKDIR /source/MoxControl
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MoxControl.dll"]