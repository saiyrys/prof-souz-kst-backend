using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading;

namespace Events.Infrastructure.Messaging.Producer
{
    public class EventProducer
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
                await _producer.ProduceAsync(_topic, message, cancellationToken);
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
    }
}
