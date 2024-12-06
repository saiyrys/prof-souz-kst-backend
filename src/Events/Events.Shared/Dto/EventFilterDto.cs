using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Shared.Dto
{
    public class EventFilterDto
    {
        public int page { get; set; } = 1;
        public string? search { get; set; } = null;
        public string? sort { get; set; } = null;
        public string? type { get; set; } = null;
        /*public DateTime? Date { get; set; }*/
       /* public bool? IsActive { get; set; }
        public bool? Status { get; set; }*/
    }
}
