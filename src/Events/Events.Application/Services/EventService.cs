using AutoMapper;
using Confluent.Kafka;
using Events.Application.Interfaces;
using Events.Domain.Builder;
using Events.Domain.Models;
using Events.Infrastructure.CacheService;
using Events.Infrastructure.Data.Repository;
using Events.Infrastructure.Messaging.Consumer;
using Events.Infrastructure.Messaging.Producer;
using Events.Shared.Dto;
using Microsoft.Extensions.Logging;
using Polly;
using System.Text.Json;

namespace Events.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IProducer<string, string> _producer; 
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly EventConsumer _consumer;
        private readonly EventProducer _eventProducer;
        private readonly DataReadyConsumer _dataReady;
        private readonly EventCache _cache;

        public EventService(IProducer<string, string> producer, IEventRepository eventRepository, 
            EventConsumer consumer, EventProducer eventProducer, DataReadyConsumer dataReady
            , IMapper mapper, EventCache cache)
        {
            _producer = producer;
            _eventRepository = eventRepository;
            _consumer = consumer;
            _cache = cache;
            _eventProducer = eventProducer;
            _mapper = mapper;

            _dataReady = dataReady;
           
        }

        public async Task<IEnumerable<GetEventDto>> GetEvents(EventFilterDto filter, CancellationToken cancellation)
        {
            int page = 1;

            await RequestCategory(null, cancellation);
            
            var events = await _eventRepository.GetEvents();
            
            var eventCategory = _consumer.GetDataCache();

            var eventDtos = events.Select(e =>
            {
                var categories = eventCategory.ContainsKey(e.EventId)
                    ? eventCategory[e.EventId]
                    : new List<string>(); // Если нет категорий, возвращаем пустой список

                return new GetEventDto
                {
                    eventId = e.EventId,
                    title = e.Title,
                    organizer = e.Organizer,
                    description = e.Description,
                    eventDate = e.EventDate.ToShortDateString(),
                    link = e.Link,
                    totalTickets = e.TotalTickets,
                    createdAt = e.CreatedAt.ToShortDateString(),
                    updatedAt = e.UpdatedAt.ToShortDateString(),
                    categories = (ICollection<string>)categories
                };
            }).ToList();

            return eventDtos;
        } 
        public async Task<GetEventDto> GetEventsByID(string eventId, CancellationToken cancellation)
        {
            var @event = await _eventRepository.GetEventById(eventId);

            await RequestCategory(eventId, cancellation);

            var eventCategory = _consumer.GetDataCache();

            var categories = eventCategory.ContainsKey(@event.EventId)
                    ? eventCategory[@event.EventId]
                    : new List<string>();

            var eventDto = new GetEventDto
            {
                    eventId = @event.EventId,
                    title = @event.Title,
                    organizer = @event.Organizer,
                    description = @event.Description,
                    eventDate = @event.EventDate.ToShortDateString(),
                    link = @event.Link,
                    totalTickets = @event.TotalTickets,
                    createdAt = @event.CreatedAt.ToShortDateString(),
                    updatedAt = @event.UpdatedAt.ToShortDateString(),
                    categories = (ICollection<string>)categories
             };


            return eventDto;

        }

        public async Task<bool> CreateEvents(CreateEventDto dto)
        {
            if (string.IsNullOrEmpty(dto.link))
            {
                dto.link = null;
            }
            
            var @event = new EventBuilder()
                .WithTitle(dto.title)
                .WithDescription(dto.description)
                .WithOrganizer(dto.organizer)
                .WithEventDate(dto.eventDate)
                .WithLink(dto.link)
                .WithTotalTickets(dto.totalTickets)
                .Build();

            
            var eventMessage = new MessagePayload
            {
                Event = @event,
                CategoriesId = dto.categoriesId // Добавляем категории сюда
            };

            var serializedMessage = JsonSerializer.Serialize(eventMessage);

            var policy = Policy.Handle<KafkaException>()
                 .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            await policy.ExecuteAsync(() => _producer.ProduceAsync("create-event-topic", new Message<string, string>
            {
                Key = @event.EventId,
                Value = serializedMessage
            }));

            if (!await _eventRepository.CreateEvents(@event))
                throw new ArgumentException("Ошибка создания события");

            return true;
        }

        public Task<bool> DeleteEvents(string eventId)
        {
            throw new NotImplementedException();
        }

        private async Task RequestCategory(string? eventId, CancellationToken cancellation)
        {
            if (!string.IsNullOrEmpty(eventId)) {
                await _eventProducer.RequestForEventCategory(eventId, cancellation);
                await _consumer.WaitForCacheUpdateAsync(cancellation);    
                return;
            }
            await _eventProducer.RequestAllCategories(cancellation);

            await _consumer.WaitForCacheUpdateAsync(cancellation);
        }

       
        

       
    }
}
