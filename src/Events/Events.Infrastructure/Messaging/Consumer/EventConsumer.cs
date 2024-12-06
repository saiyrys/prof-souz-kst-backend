using Confluent.Kafka;
using Events.Infrastructure.CacheService;
using Events.Shared.Dto;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Channels;

namespace Events.Infrastructure.Messaging.Consumer
{
    public class EventConsumer
    {
        private readonly IConsumer<string, string> _consumer;

        private readonly string _responseTopic;

        private CancellationTokenSource _cancellationTokenSource;

        private static readonly ConcurrentDictionary<string, IEnumerable<string>> _dictionary = new();
        private readonly EventCache cache = new();

        private readonly Channel<bool> _updated;

        public EventConsumer(IConfiguration configuration)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "event-response-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _responseTopic = configuration["Kafka:ResponseTopic"];

            _cancellationTokenSource = new CancellationTokenSource();

            _updated = Channel.CreateUnbounded<bool>();
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Consumer started. Waiting for message...");

            _consumer.Subscribe("event-topic-response");

            while (!cancellationToken.IsCancellationRequested)
            {
                var message = _consumer.Consume(cancellationToken);

                if (message.Topic == _responseTopic)
                {
                    ProcessMessage(message);
                }
            }
        }

        private async Task ProcessMessage(ConsumeResult<string, string> message)
        {
            try
            {
                var deserializedData = JsonSerializer.Deserialize<List<EventDto>>(message.Message.Value);

                if (deserializedData == null)
                {
                    Console.WriteLine("Received null payload.");
                    return;
                }

                foreach (var eventDto in deserializedData)
                {
                    _dictionary.AddOrUpdate(
                        eventDto.eventId,
                        eventDto.categories,
                        (key, category) => eventDto.categories
                    );

                    _updated.Writer.TryWrite(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during consumption: {ex.Message}");
            }
        }

        public ConcurrentDictionary<string, IEnumerable<string>> GetDataCache()
        {
            return _dictionary;
        }

        public async Task WaitForCacheUpdateAsync(CancellationToken cancellationToken)
        {
            while (!_dictionary.Any())
            {
                await Task.Delay(100, cancellationToken);
                Console.WriteLine("Ожидание данных...");
            }
        }




    }

}
