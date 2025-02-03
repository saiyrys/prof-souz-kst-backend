using Events.Shared.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Shared.Helpers
{
    public class QuryBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var query = bindingContext.HttpContext.Request.Query;

            var search = query.ContainsKey("search") ? query["search"].ToString() : null;

            var sort = query.ContainsKey("sort") ? query["sort"].ToString() : null;

            var page = query.ContainsKey("page") && int.TryParse(query["page"], out var p) ? p : 1;

            var type = query.ContainsKey("IsDescending") && bool.TryParse(query["IsDescending"], out var desc) ? desc : false;

            var queryDto = new EventQueryDto
            {
                Filters = new EventFilterDto { search = search },
                Sorting = new EventSortingDto { SortBy = sort, isDescending = type },
                Paging = new PagedRequestDto { page = page }
            };

            bindingContext.Result = ModelBindingResult.Success(queryDto);

            return Task.CompletedTask;
        }
    }
}
