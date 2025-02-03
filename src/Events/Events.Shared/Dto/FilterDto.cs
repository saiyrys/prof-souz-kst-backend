using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Shared.Dto
{
    public class FilterDto
    {
        public string? Search { get; set; } = null;
        /*public DateTime? Date { get; set; }*/
       /* public bool? IsActive { get; set; }
        public bool? Status { get; set; }*/
    }
    public class SortDto
    {
        public string? SortBy { get; set; } = "default";

        public bool? IsDescending { get; set; } = false;
    }

    public class PagedDto
    {
        public int page { get; set; } = 1;

        public int TotalPages { get; set; } = 18;
    }

    public class Query
    {
        public FilterDto? Filter { get; set; }

        public SortDto? Sorting { get; set; }

        public PagedDto? Paging { get; set; }
    }
}
