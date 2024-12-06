using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Shared.Dto
{
    public class CreateEventDto
    {
        public string? title { get; set; }

        public string? description { get; set; }
        public string organizer { get; set; }

        public DateTime eventDate { get; set; }

        public List<string> imagesId { get; set; }

        public string link { get; set; }

        public ICollection<string> categoriesId { get; set; }
        public int totalTickets { get; set; }

    }
}

