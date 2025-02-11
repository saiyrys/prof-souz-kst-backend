using Events.Application.Utilities.FiltrationUtill.Sort;
using Events.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces
{
    public interface ISortAction
    {
        public IEnumerable<GetEventDto> SortObject(ref IEnumerable<GetEventDto> events, SortState? sort);
    }
}
