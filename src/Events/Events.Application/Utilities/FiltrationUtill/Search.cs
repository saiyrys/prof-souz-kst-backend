using Events.Application.Interfaces;
using Events.Infrastructure.Data.Repository;
using Events.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Utilities.FiltrationUtill
{
    public class Search : ISearch
    {
        private readonly IEventRepository _eventRepository;
        public Search(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<ICollection<GetEventDto>> SearchingEvents(ICollection<GetEventDto> events, string? search = null)
        {
            IQueryable<GetEventDto> query = events.AsQueryable();

            if (search == null)
                return new List<GetEventDto>();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                query = query.Where(e =>
                    e.title.ToLower().Contains(search) ||
                    e.description.ToLower().Contains(search) ||
                    e.categories.Any(c => c.ToString().ToLower().Contains(search)) ||
                    e.totalTickets.ToString().Contains(search) ||
                    e.createdAt.ToString().Contains(search) ||
                    e.updatedAt.ToString().Contains(search));
            }

            return query.ToList();
        }
    }
}
