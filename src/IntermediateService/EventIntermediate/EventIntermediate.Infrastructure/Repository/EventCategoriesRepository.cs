using EventIntermediate.Infrastructure.Data;
using EventIntermediate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventIntermediate.Infrastructure.Repository
{
    public class EventCategoriesRepository : IEventCategoriesRepository
    {
        private readonly DataContext _context;

        public EventCategoriesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(EventCategories eventCategories)
        {
            _context.EventCategories.Add(eventCategories);

            await _context.SaveChangesAsync();
        }

        /*async Task RemoveAsync(EventCategories eventCategories)
        {
            var entity = await _context.EventCategories
                .FirstOrDefault(ec => ec.eventId == )
        }*/

        Task GetEventsWithCategories(string eventId)
        {
            throw new NotImplementedException();
        }
    }
}
