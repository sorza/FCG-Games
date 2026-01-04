# FCG-Games

Microserviço de gerenciamento de jogos escrito em .NET 8. Projeto organizado em camadas (`Api`, `Application`, `Domain`, `Infrastructure`, `Consumer`) e integrado ao Azure Service Bus para processamento de eventos assíncronos.

## Visão geral

Este repositório implementa o ciclo de vida de jogos (criação, atualização e exclusão) com arquitetura em camadas e comunicação orientada a eventos. A solução adota princípios inspirados em Domain?Driven Design (DDD) e separa responsabilidades entre apresentação, aplicação, domínio e infraestrutura.

## Tecnologias principais

- `.NET 8` (C# 12)
- `Azure Service Bus` (Topics & Subscriptions)
- `System.Text.Json` para (de)serialização
- `Microsoft.Extensions.Hosting` / `IHostedService` para o Worker Service
- `ILogger` (Microsoft.Extensions.Logging) para logging estruturado
- Injeção de dependência do ASP.NET Core (`IServiceCollection`, `IServiceScopeFactory`)
- Padrão `Repository` para abstração de persistência
- `ServiceBusProcessor` para processamento concorrente de mensagens

## Arquitetura e padrões adotados

- Camadas bem definidas:
  - `FCG-Games.Api` — apresentação / endpoints HTTP.
  - `FCG-Games.Application` — casos de uso e orquestração.
  - `FCG-Games.Domain` — entidades, agregados, enums (ex.: `EGenre`) e regras de negócio.
  - `FCG-Games.Infrastructure` — implementações de repositório e integrações externas.
  - `FCG-Games.Consumer` — Worker Service que consome eventos do Service Bus.

- Padrões e práticas:
  - DDD-inspired: entidades e regras de negócio concentradas na camada `Domain`.
  - Repository Pattern para desacoplar persistência da lógica de domínio.
  - Event-driven architecture: produtores publicam eventos (`GameCreated`, `GameUpdated`, `GameDeleted`) e consumidores aplicam mudanças de forma assíncrona.
  - Uso de `IServiceScopeFactory` dentro do consumer para resolver serviços com escopo durante o processamento de cada mensagem.
  - Tratamento explícito de conversões (ex.: `string` ? `EGenre` com `Enum.TryParse`) e validações (ex.: `Guid.TryParse`).

## Resiliência e operações idempotentes

- O consumer confirma mensagens (`CompleteMessageAsync`) somente após processamento bem-sucedido.
- Recomenda-se implementar idempotência no repositório (verificação por `AggregateId`) para evitar efeitos duplicados em reenvios de eventos.
- Configure `ServiceBusProcessorOptions` (ex.: `MaxConcurrentCalls`, `PrefetchCount`) conforme necessidade de throughput e recursos.

## Contratos e eventos

Eventos relevantes:
- `GameCreated` — payload para criação de jogo.
- `GameUpdated` — payload para atualização.
- `GameDeleted` — payload para remoção.

Os contratos estão no namespace/pacote `FCG.Shared.Contracts.Events.Domain.Games`. Garanta compatibilidade de contrato entre produtores e consumidores.

## Configuração

Recomenda-se usar __User Secrets__ em desenvolvimento e Azure Key Vault em produção.

Exemplo de configuração (`appsettings.json` ou secrets):
