using Azure.Messaging.ServiceBus;
using FCG.Shared.Contracts.Interfaces;
using System.Text.Json;

namespace FCG_Games.Infrastructure.Games.Events
{
    public class ServiceBusEventPublisher : IEventPublisher
    {
        private readonly ServiceBusClient _client;
        private readonly string _topicName;

        public ServiceBusEventPublisher(ServiceBusClient client, string topicName)
        {
            _client = client;
            _topicName = topicName;
        }

        public async Task PublishAsync<T>(T evt, string subject, string correlationId)
        {
            var sender = _client.CreateSender(_topicName);
            var body = JsonSerializer.Serialize(evt);
            var message = new ServiceBusMessage(body)
            {
                ContentType = "application/json",
                Subject = subject,
                CorrelationId = correlationId
            };

            message.ApplicationProperties["EventName"] = subject;
            message.ApplicationProperties["OccurredAt"] = DateTime.UtcNow.ToString("o");

            await sender.SendMessageAsync(message);
        }
    }
}
