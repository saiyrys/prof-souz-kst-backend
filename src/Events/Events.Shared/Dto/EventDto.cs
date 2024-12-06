using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Shared.Dto
{
    public class EventDto
    {
        public string eventId { get; set; }
        public List<string> categories { get; set; }
    }
}
