using Events.Application.Interfaces;
using Events.Infrastructure.Messaging.Consumer;
using Events.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class eventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly EventConsumer _consumer;

        public eventController(IEventService eventService, EventConsumer consumer)
        {
            _eventService = eventService;
            _consumer = consumer;
        }

        [HttpPost]
        /*[Authorize(Roles = "ADMIN, MODER")]*/
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateEvents(CreateEventDto eventsCreate)
        {
            if (eventsCreate == null)
            {
                return BadRequest();
            }

            var result = await _eventService.CreateEvents(eventsCreate);

            if (!result)
            {
                return StatusCode(500, "Ошибка при создании мероприятия");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Мероприятие успешно создан");
        }

        [HttpGet]
        /*[Authorize(Roles = "ADMIN, MODER")]*/
        [ProducesResponseType(typeof(IEnumerable<GetEventDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEvents([FromQuery]EventFilterDto filter, CancellationToken cancellation)
        {
            var result = await _eventService.GetEvents(filter, cancellation);

            /*if (!result)
            {
                return StatusCode(500, "Ошибка при создании мероприятия");
            }*/


            return Ok(result);
        }

        [HttpGet("eventId")]
        /*[Authorize(Roles = "ADMIN, MODER")]*/
        [ProducesResponseType(typeof(IEnumerable<GetEventDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEventsById(string eventId, CancellationToken cancellation)
        {
            var result = await _eventService.GetEventsByID(eventId, cancellation);

            /*if (!result)
            {
                return StatusCode(500, "Ошибка при создании мероприятия");
            }*/


            return Ok(result);
        }
    }
}
