﻿using Confluent.Kafka;
using Events.Infrastructure.CacheService;
using Events.Shared.Dto;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;

namespace Events.Infrastructure.Messaging.Consumer
{
    public class EventConsumer
    {
        private readonly IConsumer<string, string> _consumer;

        private readonly string _responseTopic;



        public EventConsumer(IConfiguration configuration)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "event-response-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _responseTopic = configuration["Kafka:ResponseTopic"];

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
                    EventCache.UpdateCache(eventDto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during consumption: {ex.Message}");
            }
        }

        public async Task<bool> WaitNotification(CancellationToken cancellation) 
        {
           
            _consumer.Subscribe(new[] { "event-data-delete-response" });

            while (!cancellation.IsCancellationRequested)
            {
                var message = _consumer.Consume(cancellation);
                
                await Task.Delay(100, cancellation);
                Console.WriteLine("Ожидание подтверждения...");

                if(message?.Message?.Value == "event data was removed" || message.Message.Value == "category not found")
                {
                    _consumer.Close();
                    return true;
                }
            }

            Console.WriteLine("Успешно...");

            

            return true;
        }
    }

}
