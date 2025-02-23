using Events.Domain.Models;
using Events.Shared.Dto;


namespace Events.Domain.Builder
{
    public class EventBuilder
    {
        private readonly Event @event;

        private bool isBuilt = false;

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

        public Event Build()
        {
            if (isBuilt)
                throw new InvalidOperationException();

            isBuilt = true;

            return @event;
        }
    }
}
