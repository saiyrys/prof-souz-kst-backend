using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.Models
{
    public class Event
    {
        public string? eventId { get; set; }

        public string? title { get; set; }

        public string? description { get; set; }

        public string? organizer { get; set; }

        public DateTime eventDate { get; set; }

        public string? link { get; set; }

        public int totalTickets { get; set; }

        public bool isActive { get; set; }

        public bool status { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }
    }
}
