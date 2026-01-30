ARG BUILD_VERSION=1.0.0
ARG BUILD_DATE
ARG VCS_REF

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY ["FCG-Games.Consumer/FCG-Games.Consumer.csproj", "FCG-Games.Consumer/"]
COPY ["FCG-Games.Application/FCG-Games.Application.csproj", "FCG-Games.Application/"]
COPY ["FCG-Games.Infrastructure/FCG-Games.Infrastructure.csproj", "FCG-Games.Infrastructure/"]
COPY ["FCG-Games.Domain/FCG-Games.Domain.csproj", "FCG-Games.Domain/"]

RUN dotnet restore "FCG-Games.Consumer/FCG-Games.Consumer.csproj"

COPY . .
WORKDIR /src/FCG-Games.Consumer

RUN dotnet publish "FCG-Games.Consumer.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    /p:InvariantGlobalization=true \
    --no-restore

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS runtime

LABEL maintainer="FCG Team" \
      version="${BUILD_VERSION}" \
      description="FCG Games Consumer - Event processor for games domain" \
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

ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "FCG-Games.Consumer.dll"]