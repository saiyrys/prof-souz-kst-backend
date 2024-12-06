using Events.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<GetEventDto>> GetEvents(EventFilterDto filter, CancellationToken cancellation);
        /*        Task<ICollection<GetEventDto>> GetEventsWithCategory();*/
        Task<GetEventDto> GetEventsByID(string eventId);
        /*        Task<bool> confirmLink(string eventId);*/
        Task<bool> CreateEvents(CreateEventDto eventsCreate);
        /*        Task<bool> UpdateEvents(string eventId, UpdateEventDto updateEvent);*/
        Task<bool> DeleteEvents(string eventId);
    }
}
