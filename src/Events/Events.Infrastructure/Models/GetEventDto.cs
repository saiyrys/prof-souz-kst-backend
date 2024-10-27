using Category.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.Models
{
    public class GetEventDto
    {
        public string eventId { get; set; }

        public string? title { get; set; }
        public string organizer { get; set; }
        public string? description { get; set; }
        public string eventDate { get; set; }
        public string link { get; set; }
        public ICollection<Categories> categories { get; set; }
        public bool isActive { get; set; }
        public bool status { get; set; }
        public int totalTickets { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
}
