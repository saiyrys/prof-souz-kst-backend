using EventIntermediate.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventIntermediate.Infrastructure.Repository
{
    public interface IEventCategoriesRepository
    {
        Task AddAsync(EventCategories eventCategories);

        /*Task RemoveAsync(EventCategories eventCategories);
*/
        Task GetEventsWithCategories(string eventId);
    }
}
