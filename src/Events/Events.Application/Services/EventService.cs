using AutoMapper;
using Confluent.Kafka;
using Events.Application.Interfaces;
using Events.Application.Utilities.PaginationUtil;
using Events.Domain.Builder;
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
        private readonly EventConsumer _consumer;
        private readonly EventProducer _eventProducer;
        private readonly EventDataReadyConsumer _dataReady;


        public EventService(IProducer<string, string> producer, IEventRepository eventRepository, 
            EventConsumer consumer, EventProducer eventProducer, EventDataReadyConsumer dataReady
            , IMapper mapper)
        {
            _producer = producer;
            _eventRepository = eventRepository;
            _consumer = consumer;

            _eventProducer = eventProducer;
            _mapper = mapper;

            _dataReady = dataReady;
           
        }

        public async Task<(IEnumerable<GetEventDto>, int TotalPages)> GetEvents(EventQueryDto query, CancellationToken cancellation)
        {
            await RequestCategory(null, cancellation);
            
            var events = await _eventRepository.GetEvents();
            
            var eventCategory = EventCache.GetDataCache();

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

            var search = EventFilterExtensions.FiltrationEvents(eventDtos, query);

            var pagination = Pagination.PaginationItem(eventDtos, query.Paging.page, query.Paging.PageSize);
            eventDtos = pagination.Item1;

            var totalPages = pagination.Item2;

            return (eventDtos, totalPages);
        } 












        public async Task<GetEventDto> GetEventsByID(string eventId, CancellationToken cancellation)
        {
            var @event = await _eventRepository.GetEventById(eventId);

            await RequestCategory(eventId, cancellation);

            var eventCategory = EventCache.GetDataCache();

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

                if (!isEventSaved)
                    throw new Exception("Ошибка записи в таблицу");
            });

            if (!isEventSaved)
            {
                throw new InvalidOperationException("Не удалось записать данные после всех попыток");
            }

            await SendEventDataToKafka(eventMessage);

            return true;
        }

        public async Task<bool> DeleteEvents(string eventId, CancellationToken cancellation)
        {
            await _eventProducer.RequestForDeleteEventDataAsync(eventId, cancellation);

            await _consumer.WaitNotification(cancellation);

            await _eventRepository.DeleteEvents(eventId);

            return true;
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
                await EventCache.WaitForCacheUpdateAsync(cancellation);    
                return;
            }
            await _eventProducer.RequestAllCategories(cancellation);

            await EventCache.WaitForCacheUpdateAsync(cancellation);
        } 
    }
}
