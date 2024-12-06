
using Events.Infrastructure.CacheService;
using Events.Infrastructure.Messaging.Consumer;

namespace Events.API.HostedService
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHostApplicationLifetime _lifeTime;

        public ConsumerHostedService(IServiceScopeFactory serviceScopeFactory, IHostApplicationLifetime lifeTime)
        {
            _serviceScopeFactory = serviceScopeFactory;

            _lifeTime = lifeTime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _lifeTime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var consumer = scope.ServiceProvider.GetRequiredService<EventConsumer>();
                       

                        await consumer.StartConsumingAsync(stoppingToken);

                    }
                    catch (Exception ex)
                    {

                    }
                }, stoppingToken);
            });
            
        }
    }
}
