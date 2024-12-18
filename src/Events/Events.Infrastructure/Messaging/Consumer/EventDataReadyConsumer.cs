using Confluent.Kafka;
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
                { _createEventTopic, cancellation => WaitEventConfirmation(cancellation) },
            };
        }

        public async Task<bool> WaitEventConfirmation(CancellationToken cancellation)
        {
            var tcs = new TaskCompletionSource<bool>();

            _consumer.Subscribe("create-event-topic");

            _ = Task.Run(() =>
            {
                try
                {
                    while (!cancellation.IsCancellationRequested)
                    {
                        var message = _consumer.Consume(cancellation);

                        if (message?.Message.Value == "creation canceled")
                            tcs.SetResult(false);
                        else
                            tcs.SetResult(true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, cancellation);

            return await tcs.Task;
  
        }
       
    }
}
