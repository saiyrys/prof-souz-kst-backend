using Events.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces
{
    public interface ISearch
    {
        Task<ICollection<GetEventDto>> SearchingEvents(ICollection<GetEventDto> events, string search);
    }
}
