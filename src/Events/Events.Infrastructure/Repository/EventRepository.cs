using Events.Infrastructure.Data;
using Events.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEvents(Event events)
        {
            _context.Add(events);

            await _context.SaveChangesAsync();

            return await SaveEvents();
        }

        public Task<bool> DeleteEvents(Event events)
        {
            throw new NotImplementedException();
        }

        public async Task<Event> GetEventById(string id)
        {
            return await _context.events.FirstOrDefaultAsync(e => e.eventId == id); 
        }

        public async Task<ICollection<GetEventDto>> GetEvents()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetEventDto>> SearchAndSortEvents(string search = null, string sort = null, string type = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateEvents(Event events)
        {
            _context.Update(events);

            await _context.SaveChangesAsync();

            return await SaveEvents();
        }
        public async Task<bool> SaveEvents()
        {
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
