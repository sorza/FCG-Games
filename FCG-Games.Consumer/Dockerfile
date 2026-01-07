FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FCG-Games.Consumer/FCG-Games.Consumer.csproj", "FCG-Games.Consumer/"]
COPY ["FCG-Games.Application/FCG-Games.Application.csproj", "FCG-Games.Application/"]
COPY ["FCG-Games.Infrastructure/FCG-Games.Infrastructure.csproj", "FCG-Games.Infrastructure/"]
COPY ["FCG-Games.Domain/FCG-Games.Domain.csproj", "FCG-Games.Domain/"]

RUN dotnet restore "FCG-Games.Consumer/FCG-Games.Consumer.csproj"

COPY . .
WORKDIR /src/FCG-Games.Consumer
RUN dotnet publish "FCG-Games.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FCG-Games.Consumer.dll"]
