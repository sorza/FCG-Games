ARG BUILD_VERSION=1.0.0
ARG BUILD_DATE
ARG VCS_REF

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY ["FCG-Games.Api/FCG-Games.Api.csproj", "FCG-Games.Api/"]
COPY ["FCG-Games.Application/FCG-Games.Application.csproj", "FCG-Games.Application/"]
COPY ["FCG-Games.Infrastructure/FCG-Games.Infrastructure.csproj", "FCG-Games.Infrastructure/"]
COPY ["FCG-Games.Domain/FCG-Games.Domain.csproj", "FCG-Games.Domain/"]

RUN dotnet restore "FCG-Games.Api/FCG-Games.Api.csproj"

COPY . .
WORKDIR /src/FCG-Games.Api

RUN dotnet publish "FCG-Games.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

LABEL maintainer="FCG Team" \
      version="${BUILD_VERSION}" \
      description="FCG Games API - Microservice for game catalog management" \
      org.opencontainers.image.created="${BUILD_DATE}" \
      org.opencontainers.image.revision="${VCS_REF}" \
      org.opencontainers.image.source="https://github.com/alexandresorza/fcg-microservices"

WORKDIR /app

RUN apk add --no-cache icu-libs && \
    addgroup -g 1000 appuser && \
    adduser -u 1000 -G appuser -s /bin/sh -D appuser && \
    chown -R appuser:appuser /app

COPY --from=build --chown=appuser:appuser /app/publish .

USER appuser

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "FCG-Games.Api.dll"]


