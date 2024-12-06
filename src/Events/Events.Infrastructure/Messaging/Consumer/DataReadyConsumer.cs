using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.Messaging.Consumer
{
    public class DataReadyConsumer
    {
        private readonly IConsumer<string, string> _consumer;

        private readonly string _notificationTopic = "data-ready";

        private TaskCompletionSource<bool> _dataReady = new TaskCompletionSource<bool>();



        public DataReadyConsumer(IConfiguration configuration)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "event-response-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public async Task StartConsumingDataReady(CancellationToken cancellationToken)
        {
            Console.WriteLine("Ожидание уведомлений о готовности данных...");

            // Подписываемся на топик, который будет присылать уведомления о готовности данных
            _consumer.Subscribe(_notificationTopic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                // Если сообщение пришло с данным ключом
                if (consumeResult.Message.Key == "event-data-ready")
                {
                    Console.WriteLine("Получено уведомление: данные готовы.");
                    // Уведомляем, что данные готовы
                    _dataReady.SetResult(true); // Сигнализируем, что данные готовы
                    ResetDataReadyTask();
                }
            }
        }
        public async Task WaitForDataReady()
        {
            await _dataReady.Task;
        }
        private void ResetDataReadyTask()
        {
            // Сбрасываем TaskCompletionSource, чтобы готовиться к следующему ожиданию
            _dataReady = new TaskCompletionSource<bool>();
        }
    }
}
