using Events.Domain.Models;
using Events.Shared.Dto;


namespace Events.Domain.Builder
{
    public class EventBuilder
    {
        private readonly Event @event;

        public EventBuilder()
        {
            @event = new Event();
        }

        

        public EventBuilder WithTitle(string title)
        {
            @event.SetTitle(title);
            return this;
        }

        public EventBuilder WithDescription(string description)
        {
            @event.SetDescription(description);
            return this;
        }

        public EventBuilder WithOrganizer(string organizer)
        {
            @event.SetOrganizer(organizer);
            return this;
        }

        public EventBuilder WithEventDate(DateTime eventDate)
        {
            @event.SetEventDate(eventDate);
            return this;
        }

        public EventBuilder WithLink(string link)
        {
            @event.SetLink(link);
            return this;
        }

        public EventBuilder WithTotalTickets(int totalTickets)
        {
            @event.SetTickets(totalTickets);
            return this;
        }

        public Event Build()
        {
            return @event;
        }
    }/*public EventBuilder FromDto(CreateEventDto dto)
        {
            @event.title = dto.title;
            @event.description = dto.description;
            @event.organizer = dto.organizer;
            @event.eventDate = dto.eventDate;
            @event.link = dto.link;
            @event.totalTickets = dto.totalTickets;

            foreach(var categoryId in dto.categoriesId)
            {
                if (string.IsNullOrWhiteSpace(categoryId))
                {
                    throw new ArgumentException("Category ID cannot be null or empty.");
                }
                
            }

            return this;
        }*/

        /*private EventBuilder GenerateId()
        {
            @event.

            return this;
        }*/
}
