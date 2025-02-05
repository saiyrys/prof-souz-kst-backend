using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Web.Http.ModelBinding;

namespace Events.Shared.Dto
{
    public class QueryDto
    {
        public string? Search { get; }

        public string? Sort { get; }

        public bool IsDescending { get; }
    }

    public class FilterDto
    {
        public DateTime? Date { get; }
        public bool? IsActive { get; }
        public bool? Status { get; }
    }


}
