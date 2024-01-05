using Microsoft.AspNetCore.Mvc;
using Test.Data;
using TestApi.Dtos;
using TestApi.Services;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ApiControllerBase<EventsController>, ICrudController<CreateEventDto, EventDto>
    {

        private readonly IService<Event, CreateEventDto, EventDto> _service;
        public EventsController(ILogger<EventsController> logger, IService<Event, CreateEventDto, EventDto> service) : base(logger)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all events.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>All the event dtos.</returns>
        /// <response code="200">No errors occurred. Events returned.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IList<EventDto>>> GetAllAsync(CancellationToken token)
        {
            try
            {
                List<EventDto> dtos = await _service.GetAllAsync(null, token);

                if (dtos.Count == 0) return NoneFoundResult;
                return dtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get an event.
        /// </summary>
        /// <param name="id">The event id.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The event dto.</returns>
        /// <response code="200">No errors occurred. Event returned.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> GetAsync(int id, CancellationToken token)
        {
            try
            {
                var returnDto = await _service.GetAsync(u => u.Id == id, token);

                if (returnDto == null) return NotFoundResult(id);
                return returnDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="dto">The create event dto.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The new event dto.</returns>
        /// <response code="201">User created.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> CreateAsync(CreateEventDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var returnDto = await _service.CreateAsync(dto, token);
                return CreatedAtAction("Create", new { id = returnDto.Id }, returnDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete an event.
        /// </summary>
        /// <param name="id">The event id.</param>
        /// <param name="token">The cancellation token.</param>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken token )
        {
            try
            {
                await _service.DeleteAsync(u => u.Id == id, token);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Replace an event.
        /// </summary>
        /// <param name="id">The event id.</param>
        /// <param name="dto">The create event dto.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The event dto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> ReplaceAsync(int id, CreateEventDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                return await _service.ReplaceAsync(u => u.Id == id, dto, token);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update an event.
        /// </summary>
        /// <param name="id">The event id.</param>
        /// <param name="dto">The create event dto.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The updated event dto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> UpdateAsync(int id, CreateEventDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                return await _service.UpdateAsync(u => u.Id == id, dto, token);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
