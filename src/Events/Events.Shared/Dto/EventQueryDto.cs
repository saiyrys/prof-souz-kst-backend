using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Web.Http.ModelBinding;

namespace Events.Shared.Dto
{
    public class QueryDto
    {
        public string? search { get; set; }
    }

    public class FilterDto
    {
        public DateTime? Date { get; }
        public bool? IsActive { get; }
        public bool? Status { get; }
    }

    public enum SortState
    {
        Current,
        AlphabeticAsc,
        AlphabeticDesc,
        DateAsc,
        DateDesc,
        TicketsAsc,
        TicketsDesc
        
    }

}
