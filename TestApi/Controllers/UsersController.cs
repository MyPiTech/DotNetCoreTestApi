using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGTest.Data;
using System.Linq.Expressions;
using TestApi.Dtos;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ApiControllerBase<UsersController>, ICrudController<CreateUserDto, UserDto>
    {
        public UsersController(ILogger<UsersController> logger, MSTestDataContext dataContext) : base(logger, dataContext)
        {
        }

        //Reusable expression tree lambda function to convert entity to dto.
        private readonly Expression<Func<User, UserDto>> toDto = u => new UserDto { 
            Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, Notes = u.Notes
        };

        //Compile the expresssion tree and use the function.
        private Func<User, UserDto> AsDto => toDto.Compile();

        private readonly Expression<Func<User, UserDto>> toDtoWithEvents = u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Notes = u.Notes,
            Events = u.Events.Select(e => new EventDto { Id = e.Id, Title = e.Title, Duration = e.Duration, Location = e.Location, Start = e.Start }).ToList()
        };

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user dto.</param>
        /// <response code="201">User created.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> CreateAsync(CreateUserDto user)
        {
            try 
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var userEntity = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Notes = user.Notes
                };
                await _dataContext.AddAsync(userEntity);
                await _dataContext.SaveChangesAsync();

                //simple logger use example
                _logger.LogInformation($"User: {string.Join(", ", user.LastName, user.FirstName)} Id:{userEntity.Id} created.");
                return CreatedAtAction("Create", new { id = userEntity.Id }, AsDto(userEntity));
            }
            catch(Exception ex)
            {
                /*
                 * Simply returning the exception message here is not ideal for a production application.
                 * Ideally the message would be logged as well as localized and abstracted for security and control reasons. 
                 */
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <response code="200">No errors occurred. Users returned.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IList<UserDto>>> GetAllAsync()
        {
            //Query syntax example.
            var dtos = await (
                from u in _dataContext.Users
                select AsDto(u)
                ).ToListAsync();

            if (dtos == null) return NotFound("No users found.");

            return dtos;
        }

        /// <summary>
        /// Get user.
        /// </summary>
        /// <param name="id">The user Id.</param>
        /// <response code="200">No errors occurred. User returned.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> GetAsync(int id)
        {
            try
            {
                var user = await _dataContext.Users
                    .Select(toDto)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null) return NotFoundResult(id);

                return user;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Overloaded get user method to include user events. Made includeEvents optional
        /// mostly to show the use of a ternary operator.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="includeEvents">Optional parameter to include user events in response.</param>
        /// <response code="200">No errors occurred. User returned.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet("Events{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> GetAsync(int userId, bool includeEvents = true)
        {
            try
            {
                var user = await _dataContext.Users
                    .Select(includeEvents? toDtoWithEvents : toDto)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) return NotFoundResult(userId);

                return user;
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove user.
        /// </summary>
        /// <param name="id">The user Id.</param>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try {
                var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null) return NotFoundResult(id);

                _dataContext.Remove(user);
                await _dataContext.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Replace user.
        /// </summary>
        /// <param name="id">The user Id.</param>
        /// <param name="user">The user dto.</param>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> ReplaceAsync(int id, CreateUserDto user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var userEntity = await _dataContext.Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (userEntity == null) return NotFoundResult(id);

                userEntity.FirstName = user.FirstName;
                userEntity.LastName = user.LastName;
                userEntity.Notes = user.Notes;

                await _dataContext.SaveChangesAsync();

                return AsDto(userEntity);   
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="id">The user Id.</param>
        /// <param name="user">The user dto.</param>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No user found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> UpdateAsync(int id, UserDto user)
        {
            try
            {
                var userEntity = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (userEntity == null) return NotFoundResult(id);

                userEntity.FirstName = user.FirstName ?? userEntity.FirstName;
                userEntity.LastName = user.LastName ?? userEntity.LastName;
                userEntity.Notes = user.Notes ?? userEntity.Notes;

                await _dataContext.SaveChangesAsync();

                return AsDto(userEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}