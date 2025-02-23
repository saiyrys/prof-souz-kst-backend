using Events.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Interface
{
    public interface IEventProducer
    {
        Task ProduceEventDataAsync(EventDto message);

        Task RequestAllCategories(CancellationToken cancellationToken);
/*
        Task RequestForEventCategory(CancellationToken cancellation);*/

        Task ProduceDeleteIntermediateAsync(string eventId, CancellationToken cancellation);
    }
}
