using Events.Application.Interfaces.IService;
using Events.Domain.Interface;
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
        public async Task<IActionResult> CreateEvents(CreateEventDto eventsCreate, CancellationToken cancellation)
        {
            if (eventsCreate == null)
            {
                return BadRequest();
            }

            var result = await _eventService.CreateEvent(eventsCreate, cancellation);

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
        public async Task<IActionResult> GetEvents(int page, CancellationToken cancellation, [FromQuery]QueryDto query, SortState sort)
        {
            var (@event, totalPages) = await _eventService.GetEvents(page, cancellation, query, sort);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return Ok(new { Items = @event, TotalPages = totalPages });
        }

        [HttpGet("eventId")]
        /*[Authorize(Roles = "ADMIN, MODER")]*/
        [ProducesResponseType(typeof(IEnumerable<GetEventDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEventsById(string eventId, CancellationToken cancellation)
        {
            var result = await _eventService.GetEventById(eventId, cancellation);

            /*if (!result)
            {
                return StatusCode(500, "Ошибка при создании мероприятия");
            }*/


            return Ok(result);
        }

        [HttpDelete("eventId")]
        /*[Authorize(Roles = "ADMIN, MODER")]*/
       /* [ProducesResponseType(typeof(IEnumerable<GetEventDto>), 200)]*/
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEventsById(string eventId, CancellationToken cancellation)
        {
            var result = await _eventService.DeleteEvent(eventId, cancellation);

            /*if (!result)
            {
                return StatusCode(500, "Ошибка при создании мероприятия");
            }*/


            return Ok(result);
        }
    }
}
