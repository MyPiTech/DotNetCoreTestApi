using Microsoft.EntityFrameworkCore;
using Test.Data;
using System.Linq.Expressions;
using TestApi.Dtos;

namespace TestApi.Services
{
    public class EventService : Service<EventService, Event, EventDto>, IService<Event, CreateEventDto, EventDto>
    {
        public EventService(ILogger<EventService> logger, MSTestDataContext dataContext) : base(logger, dataContext)
        {
            _toDto = e => new EventDto { Id = e.Id, Title = e.Title, Duration = e.Duration, Location = e.Location, Start = e.Start, UserId = e.UserId };
        }

        public async Task<List<EventDto>> GetAllAsync(Expression<Func<Event, bool>>? predicate, CancellationToken token)
        {
            return await Dtos().ToListAsync(token);
        }

        public async Task<EventDto?> GetAsync(Expression<Func<Event, bool>> predicate, CancellationToken token)
        {
            return await Dtos(predicate).FirstOrDefaultAsync(token);
        }

        public async Task<EventDto> CreateAsync(CreateEventDto dto, CancellationToken token, int? parentId = null)
        {
            if (!await ValidateParentAsync<User>(u => u.Id == dto.UserId, token)) throw new ArgumentOutOfRangeException("Invalid parent id.");

            var entity = new Event
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Location = dto.Location,
                Start = dto.Start,
                Duration = dto.Duration
            };

            await _dataContext.AddAsync(entity);
            await _dataContext.SaveChangesAsync(token);

            _logger.LogInformation($"Event: {entity.Title} Id:{entity.Id} created.");
            return AsDto(entity);
        }

        public async Task DeleteAsync(Expression<Func<Event, bool>> predicate, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentException("Invalid identifier.");

            _dataContext.Remove(entity);
            await _dataContext.SaveChangesAsync(token);
        }

        public async Task<EventDto> ReplaceAsync(Expression<Func<Event, bool>> predicate, CreateEventDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentException("Invalid identifier.");

            entity.UserId = dto.UserId;
            entity.Start = dto.Start;
            entity.Duration = dto.Duration;
            entity.Location = dto.Location;
            entity.Title = dto.Title;

            await _dataContext.SaveChangesAsync(token);

            return AsDto(entity);
        }

        public async Task<EventDto> UpdateAsync(Expression<Func<Event, bool>> predicate, CreateEventDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentException("Invalid identifier.");

            entity.UserId = dto.UserId;
            entity.Title = dto.Title;
            entity.Start = dto.Start;
            entity.Duration = dto.Duration;
            entity.Location = dto.Location ?? entity.Location;

            await _dataContext.SaveChangesAsync(token);

            return AsDto(entity);
        }
    }
}
