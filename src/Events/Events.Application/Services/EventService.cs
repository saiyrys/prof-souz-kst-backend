using AutoMapper;
using Confluent.Kafka;
using Events.Application.Interfaces;
using Events.Application.Interfaces.IService;
using Events.Domain.Builder;
using Events.Domain.Interface;
using Events.Domain.Interfaces;
using Events.Domain.Models;
using Events.Infrastructure.Data.Repositories;
using Events.Infrastructure.Messaging.Consumer;
using Events.Shared.Dto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Events.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventReaderService _readerService;
        private readonly IEventWriterService _writerService;

        public EventService(IEventReaderService readerService, IEventWriterService writerService)
        {
            _readerService = readerService;

            _writerService = writerService;
        }

        public async Task<(IEnumerable<GetEventDto>, int TotalPages)> GetEvents(int page, CancellationToken cancellation, QueryDto query, SortState sort)
        {
            return await _readerService.GetEvents(page, cancellation, query, sort);
        } 

        public async Task<GetEventDto> GetEventById(string eventId, CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(eventId))
                throw new ArgumentNullException();

            return await _readerService.GetEventById(eventId, cancellation);
        }

        public async Task<bool> CreateEvent(CreateEventDto dto, CancellationToken cancellation)
        {
            if (dto == null)
                throw new ArgumentNullException();

            return await _writerService.CreateEvent(dto, cancellation);
        }

        public async Task<bool> DeleteEvent(string eventId, CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(eventId))
                throw new ArgumentNullException();

            return await _writerService.DeleteEvent(eventId, cancellation);
        }
    }
}
