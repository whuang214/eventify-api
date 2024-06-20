using eventify_api.Interfaces;
using eventify_api.Models;
using eventify_api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace eventify_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("createEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] EventRequest request)
        {
            var eventDetails = await _eventService.GetEventDetailsAsync(request.InputText);

            var icsFileContent = IcsGenerator.GenerateIcs(eventDetails);
            var fileName = "event.ics";
            var mimeType = "text/calendar";

            return File(System.Text.Encoding.UTF8.GetBytes(icsFileContent), mimeType, fileName);
        }
    }
}
