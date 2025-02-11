using Events.Application.Interfaces;
using Events.Shared.Dto;

namespace Events.Application.Utilities.FiltrationUtill.Sort
{
    public class SortAction : ISortAction
    {
        public IEnumerable<GetEventDto> SortObject(ref IEnumerable<GetEventDto> events, SortState? sort)
        {
            switch (sort)
            {
                case SortState.AlphabeticAsc:
                    events = events.OrderBy(e => e.title);
                    break;
                case SortState.AlphabeticDesc:
                    events = events.OrderByDescending(e => e.title);
                    break;
                case SortState.DateAsc:
                    events = events.OrderBy(e => e.eventDate);
                    break;
                case SortState.DateDesc:
                    events = events.OrderByDescending(e => e.eventDate);
                    break;
                case SortState.TicketsAsc:
                    events = events.OrderBy(e => e.totalTickets);
                    break;
                case SortState.TicketsDesc:
                    events = events.OrderByDescending(e => e.totalTickets);
                    break;
                default:
                    events = events.OrderBy(e => e.title);
                    break;
            }

            return events.ToList();
        }


    }
}
