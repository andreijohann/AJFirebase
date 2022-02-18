using Firebase.Database;
using Firebase.Database.Query;
using Aj.Events.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aj.Events.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {

        private readonly ILogger<EventsController> _logger;
        private readonly AplConfig _aplConfig;
        private FirebaseClient _firebaseClient;

        public EventsController(ILogger<EventsController> logger, AplConfig aplConfig)
        {
            _logger = logger;
            _aplConfig = aplConfig;
            _firebaseClient = new FirebaseClient(aplConfig.FirebaseUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var events = await _firebaseClient.Child("Events").OrderByKey().OnceAsync<Event>();
            return new OkObjectResult(events.ToList().Select(e => e.Object));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var @event = await _firebaseClient.Child("Events").Child(id).OnceSingleAsync<Event>();

            return new OkObjectResult(@event);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Post(Event dto) 
        {
            var result = await _firebaseClient.Child("Events").PostAsync<Event>(dto);

            return new CreatedAtActionResult(nameof(GetById), null, new {id = result.Key }, dto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _firebaseClient.Child("Events").Child(id).DeleteAsync();

            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(string id, Event dto)
        {
            await _firebaseClient.Child("Events").Child(id).PutAsync<Event>(dto);

            return new NoContentResult();
        }
    }
}
