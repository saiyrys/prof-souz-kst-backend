using Category.Infrastructure.Models;
using Confluent.Kafka;
using EventIntermediate.Infrastructure.Data;
using EventIntermediate.Infrastructure.Models;
using Events.Domain.Models;
using Events.Shared.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EventIntermediate.API.Consumers
{
    public class EventConsumer
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IProducer<string, string> _producer;
        private readonly string _createEventTopic;
        private readonly string _getAllCategoryBundleTopic;
        private readonly string _getCategoryForEventTopic;
        private readonly string _deleteCategoryBundleTopic;
        private readonly string _responseTopic;
        private readonly string _dataReadyTopic;
        private readonly DataContext _context;
        
        private readonly Dictionary<string, Func<ConsumeResult<string, string>, CancellationToken, Task>> _topicHandlers;

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
            _getAllCategoryBundleTopic = "get-event-all-categories";
            _getCategoryForEventTopic = "get-event-categories";
            _deleteCategoryBundleTopic = "delete-event-categories-data";

            _context = context;

            _topicHandlers = new Dictionary<string, Func<ConsumeResult<string, string>, CancellationToken, Task>>
            {
                { _createEventTopic, CreateEventMessage },
                { _getAllCategoryBundleTopic, GetAllEventMessage },
                { _getCategoryForEventTopic, GetCategoryForEvent},
                { _deleteCategoryBundleTopic, DeleteEventCategoryBundle }
            };

            _responseTopic = "event-topic-response";
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Consumer started. Waiting for messages...");
            _consumer.Subscribe(_topicHandlers.Keys);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var message = _consumer.Consume();

                    if(_topicHandlers.TryGetValue(message.Topic, out var handler))
                    {
                        await handler(message, cancellationToken);
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

           

            var serialize = JsonSerializer.Serialize(group);
            Console.WriteLine("Serialized Response Payload: " + serialize);

            await _producer.ProduceAsync(_responseTopic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = serialize
            });
        }

        private async Task GetCategoryForEvent(ConsumeResult<string, string> message, CancellationToken cancellation)
        {
            var eventCategory = await _context.EventCategories.AsNoTracking().ToListAsync();

            var group = eventCategory
                .GroupBy(ec => ec.eventId)
                .Select(g => new
                {
                    eventId = g.Where(ec => ec.eventId == message.Message.Key).FirstOrDefault(),
                    categories = g.Select(ec => ec.categoriesId).ToList()
                });

            var serialize = JsonSerializer.Serialize(group);

            await _producer.ProduceAsync(_responseTopic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = serialize
            });
        }

        private async Task CreateEventMessage(ConsumeResult<string, string> message, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Received message: {message.Message.Value}");
                var payload = JsonSerializer.Deserialize<EventDto>(message.Message.Value);

                if (payload == null )
                {
                    Console.WriteLine("Received null payload.");
                    return; // Пропустить итерацию, если payload null
                }

                if (payload.eventId != null && payload.categories != null)
                {
                    var newEvent = payload.eventId;
                    var categoriesId = payload.categories;

                    foreach (var categoryId in categoriesId)
                    {
                        _context.EventCategories.Add(new EventCategories
                        {
                            eventId = newEvent,
                            categoriesId = categoryId

                        });

                        await _producer.ProduceAsync(_responseTopic, new Message<string, string>
                        {
                            Key = newEvent,
                            Value = categoryId
                        });
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    
                }
            }
            catch (Exception ex)
            {
                await _producer.ProduceAsync("create-event-topic", new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = "creation canceled"
                });

                throw new InvalidOperationException("Ошибка создание ивента: " + ex);
            }
        }

        public async Task DeleteEventCategoryBundle(ConsumeResult<string, string> message, CancellationToken cancellation)
        {
            var eventCategory = await _context.EventCategories
            .AsNoTracking().Where(ec => ec.eventId == message.Message.Key)
            .ToListAsync(cancellation); 

            if(eventCategory.Select(ec => ec.categoriesId) == null)
            {
                await _producer.ProduceAsync("event-data-delete-response", new Message<string, string>
                {
                    Key = message.Message.Key,
                    Value = "category not found"
                });
            }

            _context.EventCategories.RemoveRange(eventCategory);

            await _context.SaveChangesAsync(cancellation);
      
            await _producer.ProduceAsync("event-data-delete-response", new Message<string, string>
            {
                Key = message.Message.Key,
                Value = "event data was removed"
            });

            Console.WriteLine("Ответ отправлен");
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