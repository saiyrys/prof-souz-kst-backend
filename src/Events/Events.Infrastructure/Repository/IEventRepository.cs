using Events.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.Repository
{
    public interface IEventRepository
    {
        Task<ICollection<GetEventDto>> GetEvents();

        Task<Event> GetEventById(string id);

        Task<ICollection<GetEventDto>> SearchAndSortEvents(string search = null, string sort = null, string type = null);

        Task<bool> CreateEvents(Event events);
        Task<bool> UpdateEvents(Event events);
        Task<bool> DeleteEvents(Event events);
        Task<bool> SaveEvents();
    }
}
