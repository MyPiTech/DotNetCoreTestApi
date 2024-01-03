using Microsoft.EntityFrameworkCore;
using Test.Data;
using System.Linq.Expressions;
using TestApi.Dtos;

namespace TestApi.Services
{
    public class EventService : IService<CreateEventDto, EventDto>
    {
        private readonly MSTestDataContext _dataContext;
        private readonly ILogger<EventService> _logger;

        //Reusable expression tree lambda function to convert entity to dto.
        private readonly Expression<Func<Event, EventDto>> toDto = e => new EventDto { Id = e.Id, Title = e.Title, Duration = e.Duration, Location = e.Location, Start = e.Start, UserId = e.UserId };

        //Compile the expresssion tree and use the function.
        private Func<Event, EventDto> AsDto => toDto.Compile();

        public EventService(ILogger<EventService> logger, MSTestDataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<List<EventDto>> GetAllAsync(CancellationToken token)
        {
            try
            {
                //Query syntax example.
                var dtos = await(
                from e in _dataContext.Events
                select AsDto(e)
                ).ToListAsync(token);
                return dtos;
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<EventDto?> GetAsync(int id, CancellationToken token)
        {
            try
            {
                var returnDto = await _dataContext.Events
                   .Select(toDto)
                   .FirstOrDefaultAsync(u => u.Id == id, token);

                return returnDto;
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<EventDto> CreateAsync(CreateEventDto createDto, CancellationToken token)
        {
            try
            {
                var entity = new Event
                {
                    UserId = createDto.UserId,
                    Title = createDto.Title,
                    Location = createDto.Location,
                    Start = createDto.Start,
                    Duration = createDto.Duration
                };

                await _dataContext.AddAsync(entity);
                await _dataContext.SaveChangesAsync(token);

                _logger.LogInformation($"Event: {entity.Title} Id:{entity.Id} created.");
                return AsDto(entity);
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken token)
        {
            try
            {
                var entity = await _dataContext.Events.FirstOrDefaultAsync(u => u.Id == id, token);
                if (entity == null) return false;

                _dataContext.Remove(entity);
                await _dataContext.SaveChangesAsync(token);
                return true;
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<EventDto?> ReplaceAsync(int id, CreateEventDto createDto, CancellationToken token)
        {
            try
            {
                var entity = await _dataContext.Events
                    .FirstOrDefaultAsync(u => u.Id == id, token);
                if (entity == null) return null;

                entity.UserId = createDto.UserId;
                entity.Start = createDto.Start;
                entity.Duration = createDto.Duration;
                entity.Location = createDto.Location;
                entity.Title = createDto.Title;

                await _dataContext.SaveChangesAsync(token);
                return AsDto(entity);
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<EventDto?> UpdateAsync(int id, EventDto createDto, CancellationToken token)
        {
            try
            {
                var entity = await _dataContext.Events.FirstOrDefaultAsync(u => u.Id == id);

                if (entity == null) return null;

                entity.UserId = createDto.UserId ?? entity.UserId;
                entity.Start = createDto.Start ?? entity.Start;
                entity.Duration = createDto.Duration ?? entity.Duration;
                entity.Location = createDto.Location ?? entity.Location;
                entity.Title = createDto.Title ?? entity.Title;

                await _dataContext.SaveChangesAsync(token);

                return AsDto(entity);
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
