# 🎮 FCG-Games

Microsserviço de Catálogo de Jogos — Gerenciamento com Event Sourcing e sincronização distribuída.

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-green)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![Event Sourcing](https://img.shields.io/badge/Pattern-Event%20Sourcing-red)](https://martinfowler.com/eaaDev/EventSourcing.html)
[![Microservices](https://img.shields.io/badge/Architecture-Microservices-orange)](https://microservices.io/)

## 📝 Descrição

**FCG-Games** gerencia o catálogo de jogos:

- ✅ **CRUD de games**: Criar, ler, atualizar, deletar jogos
- ✅ **Event Sourcing**: Histórico imutável de alterações
- ✅ **Pub/Sub**: Publica GameDeleted para sincronizar Libraries
- ✅ **Autorização**: Admin-only para C/U/D
- ✅ **JWT**: Autenticação stateless

---

## 🚀 Pré-requisitos

- .NET 8 SDK
- SQL Server (local ou Azure)
- MongoDB (Event Store)
- Azure Service Bus
- Docker (opcional)

---

## ⚙️ Configuração Local

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GamesDb;Trusted_Connection=True;"
  },
  "ServiceBus": {
    "ConnectionString": "<service-bus-connection-string>",
    "Topics": { "Games": "games-events" }
  },
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "EventStoreDb"
  },
  "Jwt": {
    "Key": "9y4XJg0aTphzFJw3TvksRvqHXd+Q4VB8f7ZvU08N+9Q=",
    "Issuer": "FGC-Users"
  }
}
```

---

## 🚀 Como Executar

### Migrations
```bash
cd FCG-Games.Api
dotnet ef database update
```

### Local
```bash
cd FCG-Games.Api
dotnet run
# API: https://localhost:7002/swagger
```

### Consumer (Background Service)
```bash
cd FCG-Games.Consumer
dotnet run
```

---

## 📊 Endpoints

| Método | Endpoint   | Autenticação | Descrição |
|--------|------------|--------------|-----------|
| GET    | `/api`     | Qualquer     | Listar games |
| GET    | `/api/{id}`| Qualquer     | Obter game |
| POST   | `/api`     | Admin        | Criar game |
| PUT    | `/api/{id}`| Admin        | Atualizar game |
| DELETE | `/api/{id}`| Admin        | Deletar game |

---

## 🧪 Testar

```bash
# Obter token Admin
curl -X POST https://localhost:7001/api/auth \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@fcg.com",
    "password": "Senha@123"
  }'

# Criar game
curl -X POST https://localhost:7002/api \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "The Witcher 3",
    "developer": "CD Projekt Red",
    "price": 59.99,
    "releaseDate": "2015-05-19"
  }'
```

---

## 🐳 Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FCG-Games.Api/", "."]
RUN dotnet restore && dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FCG-Games.Api.dll"]
```

---

## ☸️ Kubernetes

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: fcg-games
spec:
  replicas: 3
  selector:
    matchLabels:
      app: fcg-games
  template:
    metadata:
      labels:
        app: fcg-games
    spec:
      containers:
      - name: fcg-games
        image: fcg-games:latest
        ports:
        - containerPort: 8080
        env:
        - name: ConnectionStrings__DefaultConnection
          valueFrom:
            secretKeyRef:
              name: db-secret
              key: connection-string
```

**HPA (Auto-scaling)**:
```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: fcg-games-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: fcg-games
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
```

---

## 📚 Referências

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Event Sourcing](https://martinfowler.com/eaaDev/EventSourcing.html)
- [DDD Pattern](https://www.domainlanguage.com/ddd/)

---

**FIAP Tech Challenge — Fase 4**
