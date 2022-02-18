using Firebase.Database;
using Firebase.Database.Query;
using Aj.Events.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aj.Events.WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventsController : ControllerBase
    {

        private readonly ILogger<EventsController> _logger;
        private readonly AplConfig _aplConfig;
        private readonly ChildQuery _query;

        public EventsController(ILogger<EventsController> logger, AplConfig aplConfig)
        {
            _logger = logger;
            _aplConfig = aplConfig;
            _query = new FirebaseClient(aplConfig.FirebaseUrl).Child(aplConfig.FirebaseQueryPath);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var events = await _query.OnceAsync<Event>();
            return new OkObjectResult(events.ToList().Select(e => e.Object));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var @event = await _query.Child(id).OnceSingleAsync<Event>();

            return new OkObjectResult(@event);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Post(Event dto) 
        {
            var result = await _query.PostAsync(dto);

            return new CreatedAtActionResult(nameof(GetById), null, new {id = result.Key }, dto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _query.Child(id).DeleteAsync();

            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(string id, Event dto)
        {
            await _query.Child(id).PutAsync(dto);

            return new NoContentResult();
        }
    }
}
