using Microsoft.AspNetCore.Mvc;
using Test.Data;
using TestApi.Dtos;
using TestApi.Services;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ApiControllerBase<UsersController>, ICrudController<CreateUserDto, UserDto>
    {
        private readonly IService<User, CreateUserDto, UserDto> _userService; 
        private readonly IService<Event, CreateUserEventDto, EventDto> _eventService;

        public UsersController(ILogger<UsersController> logger, IService<User, CreateUserDto, UserDto> userService, IService<Event, CreateUserEventDto, EventDto> eventService) : base(logger)
        {
            _userService = userService;
            _eventService = eventService;
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
                List<UserDto> dtos = await _userService.GetAllAsync(null, token);

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
                var returnDto = await _userService.GetAsync(u => u.Id == id, token);

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
        /// <param name="dto">The user createDto.</param>
        /// <returns>The new UserDto.</returns>
        /// <response code="201">User created.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken token)
        {
            try 
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var returnDto = await _userService.CreateAsync(dto, token);
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
                await _userService.DeleteAsync(u => u.Id == id, token);
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
        /// <param name="dto">The user createDto.</param>
        /// <returns>The UserDto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> ReplaceAsync(int id, CreateUserDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                return await _userService.ReplaceAsync(u => u.Id == id, dto, token);
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
        /// <param name="dto">The user updateDto.</param>
        /// <returns>The UserDto.</returns>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> UpdateAsync(int id, CreateUserDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                return await _userService.UpdateAsync(u => u.Id == id, dto, token);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}