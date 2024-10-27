using Category.Infrastructure.Models;
using Events.Infrastructure.Models;

namespace EventIntermediate.Infrastructure.Models
{
    public class EventCategories
    {
        public Event Event { get; set; }
        public string eventId { get; set; }

        public Categories Categories { get; set; }
        public string categoriesId { get; set; }
    }
}
