using Events.Infrastructure.Data.Repository;
using Events.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.Data.Extension
{
    public static class EventFilterExtensions
    {
        public static IQueryable<GetEventDto> FiltrationEvents(this IQueryable<GetEventDto> events, EventQueryDto queryDto)
        {
            string searchString = queryDto.Filters.search;
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                events = events.Where(e =>
                e.title.ToLower().Contains(searchString) || 
                e.description.ToLower().Contains(searchString) ||
                e.categories.Any(c => c.ToString().ToLower().Contains(searchString)) ||
                e.totalTickets.ToString().Contains(searchString) ||
                e.createdAt.ToString().Contains(searchString) ||
                e.updatedAt.ToString().Contains(searchString));
            }

            return events;
        }
    }
}
