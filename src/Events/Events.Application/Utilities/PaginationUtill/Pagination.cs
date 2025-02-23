using Events.Application.Interfaces;
using Events.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Utilities.PaginationUtil
{
    public class Pagination : IPagination
    {
        public async Task<PagedResult<T>> Paginate<T>(IEnumerable<T> items, int page = 1)
        {
            int pageSize = 18;
            int totalItems = items.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            int skip = (page - 1) * pageSize;

            var itemsForPage = items.Skip(skip).Take(pageSize).ToList();

            var result = new PagedResult<T>
            {
                Items = itemsForPage,
                TotalPages = totalPages
            };

            return result;
        }
    }
}
