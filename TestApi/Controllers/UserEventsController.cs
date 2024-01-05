using Microsoft.AspNetCore.Mvc;
using TestApi.Dtos;

namespace TestApi.Controllers
{
    public partial class UsersController : ApiControllerBase<UsersController>, ICrudController<CreateUserDto, UserDto>
    {
        /// <summary>
        /// Gets all user events.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>All the event dtos.</returns>
        /// <response code="200">No errors occurred. User events returned.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet("{id}/Events")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IList<EventDto>>> GetAllAsync(int id, CancellationToken token)
        {
            try
            {
                List<EventDto> dtos = await _eventService.GetAllAsync(e => e.UserId == id, token);

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
        /// Get a user event.
        /// </summary>     
        /// <param name="id">The user id.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The event dto.</returns>
        /// <response code="200">No errors occurred. Event returned.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet("{id}/Events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> GetAsync(int id, int eventId, CancellationToken token)
        {
            try
            {
                var returnDto = await _eventService.GetAsync(e => e.UserId == id && e.Id == eventId, token);

                if (returnDto == null) return NotFoundResult(id);
                return returnDto;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new user event.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="dto">The create user event dto.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The new event dto.</returns>
        /// <response code="201">Event created.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPost("{id}/Events")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> CreateAsync(int id, CreateUserEventDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var returnDto = await _eventService.CreateAsync(dto, token, id);
                return CreatedAtAction("Create", new { id = returnDto.Id }, returnDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete a user event.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="token">The cancellation token.</param>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpDelete("{id}/Events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAsync(int id, int eventId, CancellationToken token)
        {
            try
            {
                await _eventService.DeleteAsync(e => e.UserId == id && e.Id == eventId, token);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Replace a user event.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="dto">The create user event dto.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The event dto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPut("{id}/Events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> ReplaceAsync(int id, int eventId, CreateUserEventDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                return await _eventService.ReplaceAsync(e => e.UserId == id && e.Id == eventId, dto, token); ;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update a user event.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="dto">The create user event dto.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The updated event dto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPatch("{id}/Events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> UpdateAsync(int id, int eventId, CreateUserEventDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                return await _eventService.UpdateAsync(e => e.UserId == id && e.Id == eventId, dto, token); ;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
