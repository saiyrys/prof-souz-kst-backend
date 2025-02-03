using Confluent.Kafka;

namespace Events.Domain.Interface
{
    public interface IEventConsumer
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);

        Task<bool> WaitNotification(CancellationToken cancellation);
    }
}
