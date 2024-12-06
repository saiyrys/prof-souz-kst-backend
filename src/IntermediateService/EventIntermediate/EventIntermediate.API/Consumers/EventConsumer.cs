using Confluent.Kafka;
using EventIntermediate.Infrastructure.Data;
using EventIntermediate.Infrastructure.Models;
using Events.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EventIntermediate.API.Consumers
{
    public class EventConsumer
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IProducer<string, string> _producer;
        private readonly string _createEventTopic;
        private readonly string _getEventTopic;
        private readonly string _responseTopic;
        private readonly string _dataReadyTopic;
        private readonly DataContext _context;
        private bool _isRunning = false;

        public EventConsumer(IConfiguration configuration, DataContext context)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string, string>(producerConfig).Build();

            _createEventTopic = "create-event-topic";
            _getEventTopic = "get-event-categories";

            _dataReadyTopic = "data-ready";


            _context = context;

            _responseTopic = "event-topic-response";
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Consumer started. Waiting for messages...");
            _consumer.Subscribe(new[] { _createEventTopic, _getEventTopic });
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var message = _consumer.Consume();

                    

                    if (message.Topic == _createEventTopic)
                    {
                        // Логика для обработки create-event-topic
                        await CreateEventMessage(message, cancellationToken);
                    }
                    else if (message.Topic == _getEventTopic)
                    { 

                        await GetAllEventMessage(message,cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
            finally
            {
                _consumer.Close();
            }
        }

        private async Task GetAllEventMessage(ConsumeResult<string, string> message, CancellationToken cancellationToken)
        {
            var eventCategories = await _context.EventCategories
               .AsNoTracking()
                    .ToListAsync(cancellationToken);

            var group = eventCategories
                .GroupBy(ec => ec.eventId)
                .Select(g => new
                {
                    eventId = g.Key,
                    categories = g.Select(ec => ec.categoriesId).ToList()
                }).ToList();

            var responsePayload = new
            {
                Events = group
            };

            var serialize = JsonSerializer.Serialize(group);
            Console.WriteLine("Serialized Response Payload: " + serialize);

            await _producer.ProduceAsync(_responseTopic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = serialize
            });
        }

        private async Task CreateEventMessage(ConsumeResult<string, string> message, CancellationToken cancellationToken)
        {
            var payload = JsonSerializer.Deserialize<MessagePayload>(message.Message.Value);

            if (payload == null)
            {
                Console.WriteLine("Received null payload.");
                return; // Пропустить итерацию, если payload null
            }

            if (payload.Event != null && payload.CategoriesId != null)
            {
                var newEvent = payload.Event;
                var categoriesId = payload.CategoriesId;

                foreach (var categoryId in categoriesId)
                {
                    _context.EventCategories.Add(new EventCategories
                    {
                        eventId = newEvent.EventId,
                        categoriesId = categoryId

                    });

                    await _producer.ProduceAsync(_responseTopic, new Message<string, string>
                    {
                        Key = newEvent.EventId,
                        Value = categoryId
                    });
                }
                await _context.SaveChangesAsync(cancellationToken);

            }
        }

        private async Task SendReadyDataMessage()
        {
            string message = "Data ready";

            await _producer.ProduceAsync(_dataReadyTopic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(message)
            });
        }
    }
}