using AutoMapper;
using Events.Domain.Models;
using Events.Shared.Dto;


namespace Events.API.Mappers
{
    public class EventMap : Profile
    {
        public EventMap()
        {
            CreateMap<Event, CreateEventDto>();
            CreateMap<CreateEventDto, Event>();

            CreateMap<Event, GetEventDto>();
            CreateMap<GetEventDto, Event>();
        }
    }
}
