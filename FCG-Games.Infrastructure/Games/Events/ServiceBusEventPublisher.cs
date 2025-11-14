using Azure.Messaging.ServiceBus;
using FCG_Games.Application.Shared.Interfaces;
using System.Text.Json;

namespace FCG_Games.Infrastructure.Games.Events
{
    public class ServiceBusEventPublisher : IEventPublisher
    {
        private readonly ServiceBusClient _client;
        private readonly string _queueName;

        public ServiceBusEventPublisher(ServiceBusClient client, string queueName)
        {
            _client = client;
            _queueName = queueName;
        }

        public async Task PublishAsync<T>(T evt, string subject)
        {
            var sender = _client.CreateSender(_queueName);
            var body = JsonSerializer.Serialize(evt);
            var message = new ServiceBusMessage(body)
            {
                ContentType = "application/json",
                Subject = subject
            };
            await sender.SendMessageAsync(message);
        }
    }
}
