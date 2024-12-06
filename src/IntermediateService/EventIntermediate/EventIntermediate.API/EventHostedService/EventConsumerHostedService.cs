using EventIntermediate.API.Consumers;

namespace EventIntermediate.API.EventHostedService
{
    public class EventConsumerHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventConsumerHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            Console.WriteLine("EventConsumerHostedService initialized.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<EventConsumer>();

            await consumer.StartConsumingAsync(stoppingToken);
        }
    }
}
