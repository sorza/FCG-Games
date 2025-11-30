FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
WORKDIR /src

COPY ../FCG-Games.Api/FCG-Games.Api.csproj ./FCG-Games.Api/ 
COPY ../FCG-Games.Domain/FCG-Games.Domain.csproj ./FCG-Games.Domain/ 
COPY ../FCG-Games.Application/FCG-Games.Application.csproj ./FCG-Games.Application/ 
COPY ../FCG-Games.Infrastructure/FCG-Games.Infrastructure.csproj ./FCG-Games.Infrastructure/

RUN dotnet restore ./FCG-Games.Api/FCG-Games.Api.csproj

COPY .. .

RUN dotnet publish ./FCG-Games.Api/FCG-Games.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime WORKDIR /app COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FCG-Games.Api.dll"]
