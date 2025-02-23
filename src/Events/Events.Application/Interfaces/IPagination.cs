using Events.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces
{
    public interface IPagination
    {
        Task<PagedResult<T>> Paginate<T>(IEnumerable<T> items, int page = 1);
    }
}
