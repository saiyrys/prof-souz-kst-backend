using Events.Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Builder
{
    public class EventBuilder
    {
        private readonly Event @event;

        public EventBuilder(Event events)
        {
            @event = events;
        }

        private EventBuilder GenerateId()
        {
            @event.eventId = Guid.NewGuid().ToString();

            return this;
        }

        public EventBuilder WithTitle(string title)
        {
            @event.title = title;
            return this;
        }

        public EventBuilder WithDescription(string description)
        {
            @event.description = description;
            return this;
        }

        public EventBuilder WithOrganizer(string organizer)
        {
            @event.organizer = organizer;
            return this;
        }

        public EventBuilder WithEventDate(DateTime eventDate)
        {
            @event.eventDate = eventDate;
            return this;
        }

        public EventBuilder WithLink(string link)
        {
            @event.link = link;
            return this;
        }

        public EventBuilder WithTotalTickets(int totalTickets)
        {
            @event.totalTickets = totalTickets;
            return this;
        }

        public Event Build()
        {
            return @event;
        }
    }
}
