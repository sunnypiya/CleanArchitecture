#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY PTG.NextStep.API/PTG.NextStep.API.csproj PTG.NextStep.API/
COPY PTG.NextStep.Domain/PTG.NextStep.Domain.csproj PTG.NextStep.Domain/
COPY PTG.NextStep.Database/PTG.NextStep.Database.csproj PTG.NextStep.Database/
COPY PTG.NextStep.Service/PTG.NextStep.Service.csproj PTG.NextStep.Service/


RUN dotnet restore "PTG.NextStep.API/PTG.NextStep.API.csproj"
RUN dotnet restore "PTG.NextStep.Domain/PTG.NextStep.Domain.csproj"
RUN dotnet restore "PTG.NextStep.Database/PTG.NextStep.Database.csproj"
RUN dotnet restore "PTG.NextStep.Service/PTG.NextStep.Service.csproj"

COPY . .

WORKDIR /src/PTG.NextStep.API
RUN dotnet build -c Release -o /app/build

WORKDIR /src/PTG.NextStep.Domain
RUN dotnet build -c Release -o /app/build

WORKDIR /src/PTG.NextStep.Database
RUN dotnet build -c Release -o /app/build

WORKDIR /src/PTG.NextStep.Service
RUN dotnet build -c Release -o /app/build

# Publish the projects
WORKDIR /src/PTG.NextStep.API
RUN dotnet publish -c Release -o /app/publish

WORKDIR /src/PTG.NextStep.Domain
RUN dotnet publish -c Release -o /app/publish

WORKDIR /src/PTG.NextStep.Database
RUN dotnet publish -c Release -o /app/publish

WORKDIR /src/PTG.NextStep.Service
RUN dotnet publish -c Release -o /app/publish


# Use the .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PTG.NextStep.API.dll"]