using Microsoft.AspNetCore.Mvc;
using TestApi.Dtos;
using TestApi.Services;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ApiControllerBase<EventsController>, ICrudController<CreateEventDto, EventDto>
    {

        private readonly IService<CreateEventDto, EventDto> _service;
        public EventsController(ILogger<EventsController> logger, IService<CreateEventDto, EventDto> service) : base(logger)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all events.
        /// </summary>
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
                List<EventDto> dtos = await _service.GetAllAsync(token);

                if (dtos == null) return NotFound("No users found.");
                return dtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get event.
        /// </summary>
        /// <param name="id">The Event Id.</param>
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
                var returnDto = await _service.GetAsync(id, token);

                if (returnDto == null) return NotFoundResult(id);
                return returnDto;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="createDto">The event create dto.</param>
        /// <returns>The new event dto.</returns>
        /// <response code="201">User created.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> CreateAsync(CreateEventDto createDto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var returnDto = await _service.CreateAsync(createDto, token);
                return CreatedAtAction("Create", new { id = returnDto.Id }, returnDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Remove event.
        /// </summary>
        /// <param name="id">The Event Id.</param>
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
                try
                {
                    var success = await _service.DeleteAsync(id, token);

                    if (!success) return NotFoundResult(id);
                    return NoContent();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Replace event.
        /// </summary>
        /// <param name="id">The Event Id.</param>
        /// <param name="event">The create event dto.</param>
        /// <returns>The event dto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> ReplaceAsync(int id, CreateEventDto createDto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var returnDto = await _service.ReplaceAsync(id, createDto, token);

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
        /// Update event.
        /// </summary>
        /// <param name="id">The event Id.</param>
        /// <param name="event">The event dto.</param>
        /// <returns>The updated event dto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> UpdateAsync(int id, EventDto updateDto, CancellationToken token)
        {
            try
            {
                var returnDto = await _service.UpdateAsync(id, updateDto, token);

                if (returnDto == null) return NotFoundResult(id);
                return returnDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
