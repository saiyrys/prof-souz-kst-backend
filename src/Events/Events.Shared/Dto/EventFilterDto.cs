using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Shared.Dto
{
    public class EventFilterDto
    {
        public string? search { get; set; } = null;
        /*public DateTime? Date { get; set; }*/
       /* public bool? IsActive { get; set; }
        public bool? Status { get; set; }*/
    }

    public class EventSortingDto
    {
        public string? SortBy { get; set; } = "default";

        public bool isDescending { get; set; } = false;
    }

    public class PagedRequestDto
    {
        public int page { get; set; } = 1;

        public int PageSize { get; set; } = 18;
    }

    public class EventQueryDto
    {
        public EventFilterDto? Filters { get; set; }
        public EventSortingDto? Sorting { get; set; }
        public PagedRequestDto? Paging { get; set; }

    }


}
