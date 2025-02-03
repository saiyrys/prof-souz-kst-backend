using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces
{
    public interface IPagination
    {
        Tuple<List<T>, int> PaginationItem<T>(List<T> items, int page = 1);
    }
}
