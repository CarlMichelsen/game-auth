FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY ["GameAuth.sln", "./"]

COPY . .

RUN dotnet restore

WORKDIR "/src/."

RUN dotnet restore "./GameAuth.Api/GameAuth.Api.csproj"

RUN dotnet build "./GameAuth.Api/GameAuth.Api.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "./GameAuth.Api/GameAuth.Api.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "./GameAuth.Api.dll"]