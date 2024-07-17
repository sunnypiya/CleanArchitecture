#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY . .

WORKDIR /src/PTG.NextStep.API/
RUN dotnet restore "PTG.NextStep.API.csproj"


WORKDIR /src/PTG.NextStep.Database/
RUN dotnet restore "PTG.NextStep.Database.csproj"

WORKDIR /src/PTG.NextStep.Database/
RUN dotnet restore "PTG.NextStep.Domain.csproj"

WORKDIR /src/PTG.NextStep.Database/
RUN dotnet restore "PTG.NextStep.Services.csproj"

COPY . .
WORKDIR "/src/"
RUN dotnet build "./PTG.NextStep.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PTG.NextStep.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTG.NextStep.API.dll"]