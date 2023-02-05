#Tools
dotnet tool install --global dotnet-ef --version 7.0.1

#Entity Framework commands

## Create Migration

Run from project root

```
dotnet ef migrations add **MIGRATION NAME** --project .\GameAuth.Api\GameAuth.Api.csproj
```

Undo migration

```
dotnet ef migrations remove --project .\GameAuth.Api\GameAuth.Api.csproj
```

## Update database with migrations

Run from project root

```
dotnet ef database update --project .\GameAuth.Api\GameAuth.Api.csproj
```
