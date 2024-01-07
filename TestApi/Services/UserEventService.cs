using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test.Data;
using TestApi.Dtos;

namespace TestApi.Services
{
    public class UserEventService : Service<UserEventService, Event, EventDto>, IService<Event, CreateUserEventDto, EventDto>
    {
        public UserEventService(ILogger<UserEventService> logger, MSTestDataContext dataContext) : base(logger, dataContext)
        {
            _toDto = e => new EventDto { Id = e.Id, Title = e.Title, Duration = e.Duration, Location = e.Location, Start = e.Start, UserId = e.UserId };
        }

        public async Task<EventDto> CreateAsync(CreateUserEventDto dto, CancellationToken token, int? parentId = null)
        {
            if(parentId == null) throw new ArgumentNullException(nameof(parentId));
            if (!await ValidateParentAsync<User>(u => u.Id == parentId.Value, token)) throw new ArgumentOutOfRangeException(nameof(parentId));

            var entity = new Event
            {
                UserId = parentId.Value,
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

            if (entity == null) throw new ArgumentOutOfRangeException("Invalid identifier.");

            _dataContext.Remove(entity);
            await _dataContext.SaveChangesAsync(token);
        }

        public async Task<List<EventDto>> GetAllAsync(Expression<Func<Event, bool>>? predicate, CancellationToken token)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await Dtos(predicate).ToListAsync(token);
        }

        public async Task<EventDto?> GetAsync(Expression<Func<Event, bool>> predicate, CancellationToken token)
        {
            var dto = await Dtos(predicate).FirstOrDefaultAsync(token);
            if (dto == null) throw new ArgumentOutOfRangeException("Invalid identifier.");
            return dto;
        }

        public async Task<EventDto> ReplaceAsync(Expression<Func<Event, bool>> predicate, CreateUserEventDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);
            if (entity == null) throw new ArgumentOutOfRangeException("Invalid identifier.");

            entity.Start = dto.Start;
            entity.Duration = dto.Duration;
            entity.Location = dto.Location;
            entity.Title = dto.Title;

            await _dataContext.SaveChangesAsync(token);

            return AsDto(entity);
        }

        public async Task<EventDto> UpdateAsync(Expression<Func<Event, bool>> predicate, CreateUserEventDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentOutOfRangeException("Invalid identifier.");

            entity.Start = dto.Start;
            entity.Duration = dto.Duration;
            entity.Location = dto.Location ?? entity.Location;
            entity.Title = dto.Title ?? entity.Title;

            await _dataContext.SaveChangesAsync(token);

            return AsDto(entity);
        }

        
    }
}
