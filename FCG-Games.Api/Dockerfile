FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FCG-Games.Api/FCG-Games.Api.csproj", "FCG-Games.Api/"]
COPY ["FCG-Games.Application/FCG-Games.Application.csproj", "FCG-Games.Application/"]
COPY ["FCG-Games.Infrastructure/FCG-Games.Infrastructure.csproj", "FCG-Games.Infrastructure/"]
COPY ["FCG-Games.Domain/FCG-Games.Domain.csproj", "FCG-Games.Domain/"]
	
RUN dotnet restore "FCG-Games.Api/FCG-Games.Api.csproj"

# Copy everything and publish
COPY . .
WORKDIR /src/FCG-Games.Api
RUN dotnet publish "FCG-Games.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "FCG-Games.Api.dll"]
