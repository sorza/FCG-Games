# FCG-Games

O projeto **FCG-Games** faz parte de um ecossistema de microsserviços voltado para gerenciamento de jogos e suas bibliotecas.  
Ele foi desenvolvido com foco em **event sourcing**, **arquitetura orientada a eventos** e **integração assíncrona** via mensageria.

---

## Tecnologias Utilizadas
- **.NET 8 / ASP.NET Core** → APIs modernas e performáticas.
- **Entity Framework Core** → persistência e abstração de acesso ao banco de dados SQL Server.
- **MongoDB** → armazenamento de eventos (Event Store).
- **Azure Service Bus** → mensageria baseada em tópicos e subscriptions.
- **Docker** → containerização e execução isolada dos microsserviços.
- **Swagger / Swashbuckle** → documentação interativa da API.

---

## Arquitetura
- **Microsserviços** → cada contexto (Games, Users, Libraries, Payments) é isolado e independente.
- **Event-Driven Architecture** → comunicação entre serviços via eventos publicados em tópicos do Service Bus.
- **Event Sourcing** → todas as mudanças de estado dos jogos são registradas como eventos imutáveis.
- **Camadas bem definidas**:
  - **API** → exposição dos endpoints REST.
  - **Application** → regras de negócio e orquestração.
  - **Infrastructure** → persistência, mensageria e integrações externas.
  - **Domain** → entidades e lógica de domínio.

---

## Padrões e Designs
- **Repository Pattern** → abstração do acesso a dados.
- **Dependency Injection** → desacoplamento e facilidade de testes.
- **Middleware personalizado** → tratamento global de exceções e correlação de requisições.
- **Event Publisher/Consumer** → produtores e consumidores de eventos no Azure Service Bus.
- **Idempotência** → prevenção de duplicidade no processamento de eventos.
- **Dead Letter Queue (DLQ)** → resiliência e análise de mensagens problemáticas.

---

## Fluxo de Eventos
1. **Criação/remoção de jogos** gera eventos (`GameCreated`, `GameRemoved`).  
2. Os eventos são persistidos no **MongoDB (Event Store)**.  
3. Os eventos são publicados no **Azure Service Bus (games-topic)**.  
4. Outros microsserviços (como **Libraries**) consomem esses eventos:
   - Se um **Game** for removido → ele é automaticamente removido de todas as bibliotecas vinculadas.

---

## Observabilidade
- **Logs estruturados** com `CorrelationId` para rastrear requisições e eventos.  
- **Swagger** para documentação e testes de endpoints.  
- **GlobalExceptionMiddleware** para captura e padronização de erros.

---

## Competências demonstradas
- Microsserviços  
- Event Sourcing  
- Event-Driven Architecture (EDA)  
- Azure Service Bus  
- MongoDB (Event Store)  
- Entity Framework Core  
- .NET 8 / ASP.NET Core  
- Repository Pattern  
- Dependency Injection  
- Middleware personalizado  
- Idempotência  
- Docker  
- Swagger


## Objetivo
Este projeto foi desenvolvido como parte de um portfólio pessoal para demonstrar:
- Conhecimento em **arquitetura de microsserviços**.  
- Aplicação prática de **event sourcing** e **mensageria assíncrona**.  
- Uso de **padrões de projeto** e boas práticas de engenharia de software.  
