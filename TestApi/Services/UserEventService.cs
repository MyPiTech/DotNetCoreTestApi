using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test.Data;
using TestApi.Dtos;

namespace TestApi.Services
{
    public class UserEventService : IService<Event, CreateUserEventDto, EventDto>
    {
        private readonly MSTestDataContext _dataContext;
        private readonly ILogger<UserEventService> _logger;

        //Reusable expression tree lambda function to convert entity to dto.
        private readonly Expression<Func<Event, EventDto>> toDto = e => new EventDto { Id = e.Id, Title = e.Title, Duration = e.Duration, Location = e.Location, Start = e.Start, UserId = e.UserId };

        //Compile the expresssion tree and use the function.
        private Func<Event, EventDto> AsDto => toDto.Compile();

        private IQueryable<EventDto> Dtos(Expression<Func<Event, bool>>? predicate = null)
        {
            var baseQ = _dataContext.Events.AsQueryable();
            if (predicate != null) baseQ = baseQ.Where(predicate);
            return baseQ.Select(toDto);
        }

        public UserEventService(ILogger<UserEventService> logger, MSTestDataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<EventDto> CreateAsync(CreateUserEventDto dto, CancellationToken token, int? parentId = null)
        {
            if(parentId == null) throw new ArgumentNullException(nameof(parentId));

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

            if (entity == null) throw new ArgumentException("Invalid identifier.");

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
            if (dto == null) throw new ArgumentException("Invalid identifier.");
            return dto;
        }

        public async Task<EventDto> ReplaceAsync(Expression<Func<Event, bool>> predicate, CreateUserEventDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentException("Invalid identifier.");

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

            if (entity == null) throw new ArgumentException("Invalid identifier.");

            entity.Start = dto.Start;
            entity.Duration = dto.Duration;
            entity.Location = dto.Location ?? entity.Location;
            entity.Title = dto.Title ?? entity.Title;

            await _dataContext.SaveChangesAsync(token);

            return AsDto(entity);
        }
    }
}
