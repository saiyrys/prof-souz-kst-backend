using Events.Shared.Dto;


namespace Events.Application.Utilities.FiltrationUtill.Sort
{
    public class SortViewModel
    {
        public SortState AlphabeticSort { get; }
        public SortState EventDateSort { get; }
        public SortState TicketsSort { get; }
        public SortState CurrentSort { get; }
        public SortState Current { get; }



        public SortViewModel(SortState sortOrder)
        {
            AlphabeticSort = sortOrder == SortState.AlphabeticAsc ? SortState.AlphabeticDesc : SortState.AlphabeticAsc;

            EventDateSort = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;

            TicketsSort = sortOrder == SortState.TicketsAsc ? SortState.TicketsDesc : SortState.TicketsAsc;

            CurrentSort = sortOrder = SortState.Current;

            Current = sortOrder;
        }
    }
}
