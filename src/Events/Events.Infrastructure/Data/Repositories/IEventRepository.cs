using Events.Domain.Models;
using Events.Shared.Dto;

namespace Events.Infrastructure.Data.Repositories
{
    public interface IEventRepository
    {
        Task<ICollection<Event>> GetEvents();

        Task<Event> GetEventById(string id);

        Task<ICollection<GetEventDto>> SearchAndSortEvents(string search = null, string sort = null, string type = null);

        Task<bool> CreateEvents(Event events);

        Task<bool> CreateEventTransaction(Event @event);

        Task<bool> UpdateEvents(Event events);

        Task<bool> DeleteEvents(string eventId);

        Task<bool> SaveEvents();
    }
}
