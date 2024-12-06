using Category.Infrastructure.Models;
using Events.Infrastructure.CacheService;
using Events.Infrastructure.Messaging.Consumer;
using Events.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Events.Domain.Models;
using Events.Shared.Dto;

namespace Events.Infrastructure.Data.Repository
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
            _context.Event.Add(events);

            await _context.SaveChangesAsync();

            return await SaveEvents();
        }

        public Task<bool> DeleteEvents(Event events)
        {
            throw new NotImplementedException();
        }

        public async Task<Event> GetEventById(string id)
        {
            return await _context.Event.FirstOrDefaultAsync(e => e.EventId == id); 
        }

        public async Task<ICollection<Event>> GetEvents()
        {
            return await _context.Event.OrderBy(e => e.EventId).ToListAsync();
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
