using Microsoft.EntityFrameworkCore;
using Test.Data;
using System.Linq.Expressions;
using TestApi.Dtos;

namespace TestApi.Services
{
    public class UserService : IUserService
    {
        private readonly MSTestDataContext _dataContext;
        private readonly ILogger<UserService> _logger;

        //Reusable expression tree lambda function to convert entity to dto.
        private readonly Expression<Func<User, UserDto>> toDto = u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Notes = u.Notes
        };

        private readonly Expression<Func<User, UserDto>> toDtoWithEvents = u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Notes = u.Notes,
            Events = u.Events.Select(e => new EventDto { Id = e.Id, Title = e.Title, Duration = e.Duration, Location = e.Location, Start = e.Start }).ToList()
        };

        //Compile the expresssion tree toDto and use the function.
        private Func<User, UserDto> AsDto => toDto.Compile();

        public UserService(ILogger<UserService> logger, MSTestDataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken token)
        {
            try {
                //Query syntax example.
                var dtos = await (
                    from u in _dataContext.Users
                    select AsDto(u)
                    ).ToListAsync(token);
                return dtos;
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw; 
            }
            
        }

        public async Task<UserDto?> GetAsync(int id, CancellationToken token)
        {
            try
            {
                var returnDto = await _dataContext.Users
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

        public async Task<UserDto?> GetAsync(int id, CancellationToken token, bool includeEvents = true)
        {
            try
            {
                var returnDto = await _dataContext.Users
                    .Select(includeEvents ? toDtoWithEvents : toDto)
                    .FirstOrDefaultAsync(u => u.Id == id, token);

                return returnDto;
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw;
            }

        }

        public async Task<UserDto> CreateAsync(CreateUserDto createDto, CancellationToken token)
        {
            try
            {
                var entity = new User
                {
                    FirstName = createDto.FirstName,
                    LastName = createDto.LastName,
                    Notes = createDto.Notes
                };
                await _dataContext.AddAsync(entity, token);
                await _dataContext.SaveChangesAsync(token);

                _logger.LogInformation($"User: {string.Join(", ", createDto.LastName, createDto.FirstName)} Id:{entity.Id} created.");
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
                var entity = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id, token);
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

        public async Task<UserDto?> ReplaceAsync(int id, CreateUserDto createDto, CancellationToken token)
        {
            try
            {
                var entity = await _dataContext.Users
                    .FirstOrDefaultAsync(u => u.Id == id, token);

                if (entity == null) return null;

                entity.FirstName = createDto.FirstName;
                entity.LastName = createDto.LastName;
                entity.Notes = createDto.Notes;

                await _dataContext.SaveChangesAsync(token);

                return AsDto(entity);
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<UserDto?> UpdateAsync(int id, UserDto updateDto, CancellationToken token)
        {
            try
            {
                var entity = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id, token);

                if (entity == null) return null;

                //Ternary operators might be more efficient...
                entity.FirstName = updateDto.FirstName ?? entity.FirstName;
                entity.LastName = updateDto.LastName ?? entity.LastName;
                entity.Notes = updateDto.Notes ?? entity.Notes;

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
