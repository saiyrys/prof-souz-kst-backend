using Confluent.Kafka;
using Events.Domain.Interface;
using Events.Shared.Dto;
using Microsoft.Extensions.Configuration;
using System.Text.Json;


namespace Events.Infrastructure.Messaging.Producer
{
    public class EventProducer : IEventProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public EventProducer(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _topic = "get-event-categories";
        }

        public async Task SendCreateEventCategoryAsync(EventDto message)
        {
            var data = JsonSerializer.Serialize(message);

            await _producer.ProduceAsync("create-event-topic", new Message<string, string>
            {
                Key = message.eventId,
                Value = data
            });
        }

        public async Task RequestAllCategories(CancellationToken cancellationToken)
        {
            var message = new Message<string, string>
            {
                Key = "request-categories",
                Value = "get-all-categories" // Мы просто отправим запрос без конкретных параметров
            };

            try
            {
                // Отправляем сообщение в топик для запроса категорий
                await _producer.ProduceAsync("get-event-all-categories", message, cancellationToken);
                Console.WriteLine("Request for all event categories sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to Kafka: {ex.Message}");
            }
        }

        public async Task RequestForEventCategory(string eventId, CancellationToken cancellation)
        {
            var message = new Message<string, string>
            {
                Key = eventId,
                Value = "get-category"
            };

            await _producer.ProduceAsync(_topic, message, cancellation);
        }

        public async Task RequestForDeleteEventDataAsync(string eventId, CancellationToken cancellation)
        {
            var message = new Message<string, string>
            {
                Key = eventId,
                Value = "delete-category"
            };

            await _producer.ProduceAsync("delete-event-categories-data", message, cancellation);
        }
    }
}
