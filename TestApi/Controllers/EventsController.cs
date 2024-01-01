using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGTest.Data;
using System.Linq.Expressions;
using TestApi.Dtos;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ApiControllerBase<EventsController>, ICrudController<CreateEventDto, EventDto>
    {
        public EventsController(ILogger<EventsController> logger, PGTestDataContext dataContext) : base(logger, dataContext)
        {
        }

        //Reusable expression tree lambda function to convert entity to dto.
        private readonly Expression<Func<Event, EventDto>> toDto = e => new EventDto { Id = e.Id, Title = e.Title, Duration = e.Duration, Location = e.Location, Start = e.Start, UserId = e.UserId };
        
        //Compile the expresssion tree and use the function.
        private Func<Event, EventDto> AsDto => toDto.Compile();

        /// <summary>
        /// Creates a new event event.
        /// </summary>
        /// <param name="event">The event dto.</param>
        /// <response code="201">User created.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> CreateAsync(CreateEventDto @event)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var eventEntity = new Event
                {
                    UserId = @event.UserId,
                    Title = @event.Title,
                    Location = @event.Location,
                    Start = @event.Start,
                    Duration = @event.Duration
                };

                await _dataContext.AddAsync(eventEntity);
                await _dataContext.SaveChangesAsync();

                //simple logger use example
                _logger.LogInformation($"Event: {eventEntity.Title} Id:{eventEntity.Id} created.");
                return CreatedAtAction("Create", new { id = eventEntity.Id }, AsDto(eventEntity));
            }
            catch (Exception ex)
            {
                /*
                 * Simply returning the exception message here is not ideal for a production application.
                 * Ideally the message would be logged as well as localized and abstracted for security and control reasons. 
                 */
                return BadRequest(ex.Message);
            }
        }

        /*
         Given this method takes no parameters always returns the same type and is relatively error proof. 
         It is an ideal method to present as an expression example. 
         */
        /// <summary>
        /// Gets all events.
        /// </summary>
        /// <response code="200">No errors occurred. Users returned.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IList<EventDto>> GetAllAsync() => await _dataContext.Events
            .Select(toDto)
            .ToListAsync();

        /// <summary>
        /// Get event.
        /// </summary>
        /// <param name="id">The Event Id.</param>
        /// <response code="200">No errors occurred. Event returned.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> GetAsync(int id)
        {
            try
            {
                var @event = await _dataContext.Events
                    .Select(toDto)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (@event == null) return NotFoundResult(id);

                return @event;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var @event = await _dataContext.Events.FirstOrDefaultAsync(u => u.Id == id);

                if (@event == null) return NotFoundResult(id);

                _dataContext.Remove(@event);
                await _dataContext.SaveChangesAsync();

                return NoContent();
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
        /// <param name="event">The event dto.</param>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> ReplaceAsync(int id, CreateEventDto @event)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var eventEntity = await _dataContext.Events
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (eventEntity == null) return NotFoundResult(id);

                eventEntity.UserId = @event.UserId;
                eventEntity.Start = @event.Start;
                eventEntity.Duration = @event.Duration;
                eventEntity.Location = @event.Location;
                eventEntity.Title = @event.Title;

                await _dataContext.SaveChangesAsync();

                return AsDto(eventEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="id">The event Id.</param>
        /// <param name="event">The event dto.</param>
        /// <response code="204">No errors occurred.</response>
        /// <response code="404">No event found.</response>
        /// <response code="400">Unanticipated error occurred.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> UpdateAsync(int id, EventDto @event)
        {
            try
            {
                var eventEntity = await _dataContext.Events.FirstOrDefaultAsync(u => u.Id == id);

                if (eventEntity == null) return NotFoundResult(id);

                eventEntity.UserId = @event.UserId ?? eventEntity.UserId;
                eventEntity.Start = @event.Start ?? eventEntity.Start;
                eventEntity.Duration = @event.Duration ?? eventEntity.Duration;
                eventEntity.Location = @event.Location ?? eventEntity.Location  ;
                eventEntity.Title = @event.Title ?? eventEntity.Title;

                await _dataContext.SaveChangesAsync();

                return AsDto(eventEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
