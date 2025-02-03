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
        Task SendCreateEventCategoryAsync(EventDto message);

        Task RequestAllCategories(CancellationToken cancellationToken);

        Task RequestForEventCategory(string eventId, CancellationToken cancellation);

        Task RequestForDeleteEventDataAsync(string eventId, CancellationToken cancellation);
    }
}
