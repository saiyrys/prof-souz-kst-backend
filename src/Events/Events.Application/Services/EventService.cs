using AutoMapper;
using Confluent.Kafka;
using Events.Application.Interfaces;
using Events.Application.Utilities.PaginationUtil;
using Events.Domain.Builder;
using Events.Domain.Interface;
using Events.Domain.Interfaces;

//using Events.Domain.Interfaces;
using Events.Domain.Models;
using Events.Infrastructure.CacheService;
using Events.Infrastructure.Data.Extension;
using Events.Infrastructure.Data.Repository;
using Events.Infrastructure.Messaging.Consumer;
using Events.Infrastructure.Messaging.Producer;
using Events.Shared.Dto;
using Microsoft.Extensions.Logging;
using Polly;

namespace Events.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IProducer<string, string> _producer; 
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IPagination _pagination;
     
        private readonly IEventProducer _eventProducer;
        private readonly IEventDataReadyConsumer _dataReady;
        private readonly IEventCache _cache;


        public EventService(IProducer<string, string> producer, IEventRepository eventRepository,
             IEventProducer eventProducer, IEventDataReadyConsumer dataReady
            ,IMapper mapper, IPagination pagination, IEventCache cache)
        {
            _producer = producer;
            _eventRepository = eventRepository;
            _eventProducer = eventProducer;
            _mapper = mapper;
            _dataReady = dataReady;
            _pagination = pagination;
            _cache = cache;
        }

        public async Task<(IEnumerable<GetEventDto>, int TotalPages)> GetEvents(EventQueryDto query, CancellationToken cancellation)
        {
            var eventDto = await GetCategoryFromCache(null, cancellation);

            var paginationItem = _pagination.PaginationItem(eventDto, query.Paging.page = 1);

            eventDto = paginationItem.Item1;
            int totalPages = paginationItem.Item2;

            return (eventDto, totalPages);
        } 

        public async Task<GetEventDto> GetEventsByID(string eventId, CancellationToken cancellation)
        {
            var eventDto = await GetCategoryFromCache(eventId, cancellation);

            var @event = eventDto.FirstOrDefault(e => e.eventId == eventId);

            return @event;
        }

        public async Task<bool> CreateEvents(CreateEventDto dto, CancellationToken cancellation)
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


            var eventMessage = new EventDto
            {
                eventId = @event.EventId,
                categories = dto.categoriesId.ToList() // Добавляем категории сюда
            };

            var retryPolicy = Policy.Handle<KafkaException>()
                 .WaitAndRetryAsync(3, retryAttempt =>
                 TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));


            var isEventSaved = true;

            await retryPolicy.ExecuteAsync(async () =>
            {
                isEventSaved = await _eventRepository.CreateEventTransaction(@event);

                await SendEventDataToKafka(eventMessage);
            });

            if (!isEventSaved)
            {
                throw new InvalidOperationException("Не удалось записать данные после всех попыток");
            }

            return true;
        }

        public async Task<bool> DeleteEvents(string eventId, CancellationToken cancellation)
        {
            await _eventProducer.RequestForDeleteEventDataAsync(eventId, cancellation);

            await _dataReady.WaitNotificationForEventWasDeleted(cancellation);

            await _eventRepository.DeleteEvents(eventId);

            return true;
        }

        private async Task<List<GetEventDto>> GetCategoryFromCache(string? eventId, CancellationToken cancellation)
        {
            await RequestCategory(null, cancellation);

            var events = await _eventRepository.GetEvents();

            var eventCategory = _cache.GetDataCache();

            var eventDtos = events.Select(e =>
            {
                var categories = eventCategory.ContainsKey(e.EventId)
                        ? eventCategory[e.EventId]
                        : new List<string>();

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

        private async Task<bool> SendEventDataToKafka(EventDto eventMessage)
        {
            try
            {
                await _eventProducer.SendCreateEventCategoryAsync(eventMessage);

                return true;
            }
            catch (Exception ex)
            {
                await _eventRepository.DeleteEvents(eventMessage.eventId); 
                throw new InvalidOperationException("Ошибка отправки в Kafka. Откат действий.", ex);
            }
        }

        private async Task RequestCategory(string? eventId, CancellationToken cancellation)
        {
            if (!string.IsNullOrEmpty(eventId)) {
                await _eventProducer.RequestForEventCategory(eventId, cancellation);
                await _cache.WaitForCacheUpdateAsync(cancellation);    
                return;
            }
            await _eventProducer.RequestAllCategories(cancellation);

            await _cache.WaitForCacheUpdateAsync(cancellation);
        } 
    }
}
