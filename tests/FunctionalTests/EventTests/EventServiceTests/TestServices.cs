/*using AutoMapper;
using Confluent.Kafka;
using Events.Application.Services;
using Events.Infrastructure.Dto;
using Events.Infrastructure.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationalTests.EventTests.EventServiceTests
{
    public class TestServices
    {
        private readonly Mock<IProducer<string, string>> _producerMock;
        private readonly Mock<IEventRepository> _eventRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly EventService _eventService;

        public TestServices()
        {
            _producerMock = new Mock<IProducer<string, string>>();
            _eventRepository = new Mock<IEventRepository>();
            _mapper = new Mock<IMapper>();
            

            _eventService = new EventService(_producerMock.Object, _eventRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateEvent_ShouldSendMessageToKafka()
        {
            // Arrange
            var createEventDto = new CreateEventDto
            {
                title = "Test Event",
                description = "This is a test event.",
                organizer = "Test Organizer",
                link = "", // или можно указать значение
                totalTickets = 100,
                categoriesId = new List<string> { "category1", "category2" }
            };

            // Act
            await _eventService.CreateEvents(createEventDto);

            // Assert
            _producerMock.Verify(p => p.ProduceAsync(
                "create-event-topic",
                It.IsAny<Message<string, string>>(),
                default), Times.Once);
        }
    }
}
*/