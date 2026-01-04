using Azure.Messaging.ServiceBus;
using FCG.Shared.Contracts.Events.Domain.Games;
using FCG_Games.Application.Shared.Interfaces;
using FCG_Games.Domain.Games.Entities;
using FCG_Games.Domain.Games.Enums;
using System.Text.Json;

namespace FCG_Games.Consumer.Consumers
{
    public class GamesTopicConsumer : IHostedService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<GamesTopicConsumer> _logger;

        public GamesTopicConsumer(ServiceBusClient client, IConfiguration cfg, IServiceScopeFactory scopeFactory, ILogger<GamesTopicConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            var topicName = cfg["ServiceBus:Topics:Games"];
            var subscriptionName = cfg["ServiceBus:Subscriptions:Games"];

            _processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 4,
                PrefetchCount = 20
            });

            _processor.ProcessMessageAsync += OnMessageAsync;
            _processor.ProcessErrorAsync += OnErrorAsync;
        }       

        private async Task OnMessageAsync(ProcessMessageEventArgs args)
        {
            var subject = args.Message.Subject;
            var body = args.Message.Body.ToString();

            _logger.LogInformation("Mensagem recebida: Subject={Subject}, CorrelationId={CorrelationId}", subject, args.Message.CorrelationId);

            switch(subject)
            {
                case "GameCreated":
                    await HandleGameCreatedEvent(body);
                    break;
                case "GameDeleted":
                    await HandleGameDeletedEvent(body);
                    break;
                case "GameUpdated":
                    await HandleGameUpdatedEvent(body);
                    break;
                default:
                    _logger.LogWarning("Evento desconhecido: {Subject}", subject);
                    break;
            }

            await args.CompleteMessageAsync(args.Message);
        }

        private async Task HandleGameUpdatedEvent(string body)
        {          
            var evt = JsonSerializer.Deserialize<GameUpdatedEvent>(body);

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGameRepository>();
           
            if (!Guid.TryParse(evt!.AggregateId, out var gameId))
                throw new FormatException($"AggregateId inválido: {evt.AggregateId}");
           
            var game = await repo.GetByIdAsync(gameId);

            if (game is not null)
            {  
                game.Update(evt.Title, evt.Price, evt.LaunchYear, evt.Developer, Enum.Parse<EGenre>(evt.Genre));          
                await repo.UpdateAsync(game);
            }
        }

        private async Task HandleGameDeletedEvent(string body)
        {
            var evt = JsonSerializer.Deserialize<GameDeletedEvent>(body);

            using var scope = _scopeFactory.CreateScope();

            var repo = scope.ServiceProvider.GetRequiredService<IGameRepository>();
            var gameId = Guid.Parse(evt!.AggregateId);

            var game = await repo.GetByIdAsync(gameId);
            if (game is not null)
                await repo.DeleteAsync(game.Id);

        }

        private async Task HandleGameCreatedEvent(string body)
        {
            var evt = JsonSerializer.Deserialize<GameCreatedEvent>(body);

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGameRepository>();

            var game = Game.Create(evt!.Title, evt.Price, evt.LaunchYear, evt.Developer, Enum.Parse<EGenre>(evt.Genre));

            if (!await repo.Exists(game))
                await repo.AddAsync(game);
        }

        private Task OnErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Erro no consumer: {EntityPath}", args.EntityPath);
            return Task.CompletedTask;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consumer iniciado para {Topic}/{Subscription}", _processor.EntityPath, "games-api-sub");
            await _processor.StartProcessingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consumer parado");
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
        }
    }

}
