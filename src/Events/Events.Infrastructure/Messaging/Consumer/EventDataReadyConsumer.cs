using Confluent.Kafka;
using Events.Domain.Interface;
using Events.Shared.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Events.Infrastructure.Messaging.Consumer
{
    public class EventDataReadyConsumer
    { 
        private readonly IConsumer<string, string> _consumer;

        private readonly Dictionary<string, Func<CancellationToken,Task>> _topicHandlers;

        private readonly string _createEventTopic = "create-event-topic";

        private readonly string _deleteEventTopic = "delete-event-topic";

        public EventDataReadyConsumer(IConfiguration configuration)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "event-response-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();

            _topicHandlers = new Dictionary<string, Func<CancellationToken, Task>>
            {
                { _createEventTopic, cancellation => CreateTopic(cancellation) },
            };
        }

        public async Task<bool> CreateTopic(CancellationToken cancellation)
        {
            _consumer.Subscribe("create-event-topic");
           
            try
            {
                while (!cancellation.IsCancellationRequested)
                {
                    var message = _consumer.Consume(cancellation);

                    await Task.Delay(100, cancellation);
                    Console.WriteLine("Ожидание подтверждения...");

                    if(message?.Message.Value == "Creation Success")
                    {
                        _consumer.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public async Task<bool> WaitNotificationForEventWasDeleted(CancellationToken cancellation)
        {
            _consumer.Subscribe( "event-delete-response" );

            while (!cancellation.IsCancellationRequested)
            {
                var message = _consumer.Consume(cancellation);

                await Task.Delay(100, cancellation);
                Console.WriteLine("Ожидание подтверждения...");

                if (message?.Message?.Value == "event data was removed" || message.Message.Value == "category not found")
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
