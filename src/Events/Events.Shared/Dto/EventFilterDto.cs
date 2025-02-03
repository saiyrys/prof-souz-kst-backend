using Events.Shared.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Web.Http.ModelBinding;

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
        [JsonPropertyName("sort")]
        public string? SortBy { get; set; } = "default";

        [JsonPropertyName("type")]
        public bool isDescending { get; set; } = false;
    }

    public class PagedRequestDto
    {
        [JsonPropertyName("page")]
        public int page { get; set; } = 1;
    }

    [ModelBinder(BinderType = typeof(QuryBinder))]
    public class EventQueryDto
    {
        [JsonPropertyName("search")]
        public EventFilterDto? Filters { get; set; }

        public EventSortingDto? Sorting { get; set; }

        public PagedRequestDto? Paging { get; set; }

    }


}
