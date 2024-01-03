using Microsoft.AspNetCore.Mvc;
using TestApi.Dtos;
using TestApi.Services;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ApiControllerBase<UsersController>, ICrudController<CreateUserDto, UserDto>
    {
        private readonly IUserService _service;

        public UsersController(ILogger<UsersController> logger, IUserService service) : base(logger)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>All the UserDtos.</returns>
        /// <response code="200">No errors occurred. Users returned.</response>
        /// <response code="404">No users found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IList<UserDto>>> GetAllAsync(CancellationToken token)
        {
            try
            {
                List<UserDto> dtos = await _service.GetAllAsync(token);

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
        /// Get user.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns>The UserDto.</returns>
        /// <response code="200">No errors occurred. User returned.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> GetAsync(int id, CancellationToken token)
        {
            try
            {
                var returnDto = await _service.GetAsync(id, token);

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
        /// Overloaded get user method to include user events. Made includeEvents optional
        /// mostly to show the use of a ternary operator in the service.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="includeEvents">Optional parameter to include user events in response.</param>
        /// <returns>The UserDto.</returns>
        /// <response code="200">No errors occurred. User returned.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet("Events{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> GetAsync(int id, CancellationToken token, bool includeEvents = true)
        {
            try
            {
                var returnDto = await _service.GetAsync(id, token, includeEvents);

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
        /// Creates a new user.
        /// </summary>
        /// <param name="createDto">The user createDto.</param>
        /// <returns>The new UserDto.</returns>
        /// <response code="201">User created.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> CreateAsync(CreateUserDto createDto, CancellationToken token)
        {
            try 
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var returnDto = await _service.CreateAsync(createDto, token);
                return CreatedAtAction("Create", new { id = returnDto.Id }, returnDto);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }



        /// <summary>
        /// Remove user.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken token)
        {
            try {
                var success = await _service.DeleteAsync(id, token);

                if(!success) return NotFoundResult(id);
                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Replace user.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="createDto">The user createDto.</param>
        /// <returns>The UserDto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> ReplaceAsync(int id, CreateUserDto createDto, CancellationToken token)
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
        /// Update user.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="updateDto">The user updateDto.</param>
        /// <returns>The UserDto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> UpdateAsync(int id, UserDto updateDto, CancellationToken token)
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