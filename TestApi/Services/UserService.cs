using Microsoft.EntityFrameworkCore;
using Test.Data;
using System.Linq.Expressions;
using TestApi.Dtos;

namespace TestApi.Services
{
    public class UserService : IService<User, CreateUserDto, UserDto>
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

        //Compile the expresssion tree toDto and use the function.
        private Func<User, UserDto> AsDto => toDto.Compile();

        private IQueryable<UserDto> Dtos(Expression<Func<User, bool>>? predicate = null)
        {
            var baseQ = _dataContext.Users.AsQueryable();
            if (predicate != null) baseQ = baseQ.Where(predicate);
            return baseQ.Select(toDto);
        }

        public UserService(ILogger<UserService> logger, MSTestDataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<List<UserDto>> GetAllAsync(Expression<Func<User, bool>>? predicate, CancellationToken token)
        {
            return await Dtos().ToListAsync(token);
        }

        public async Task<UserDto?> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken token)
        {
            return await Dtos(predicate).FirstOrDefaultAsync(token);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken token, int? parentId = null)
        {
            var entity = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Notes = dto.Notes
            };
            await _dataContext.AddAsync(entity, token);
            await _dataContext.SaveChangesAsync(token);

            _logger.LogInformation($"User: {string.Join(", ", dto.LastName, dto.FirstName)} Id:{entity.Id} created.");
            return AsDto(entity);
        }

        public async Task DeleteAsync(Expression<Func<User, bool>> predicate, CancellationToken token)
        {
            var entity = await _dataContext.Users.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentException("Invalid identifier.");

            _dataContext.Remove(entity);
            await _dataContext.SaveChangesAsync(token);
        }

        public async Task<UserDto> ReplaceAsync(Expression<Func<User, bool>> predicate, CreateUserDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Users.FirstOrDefaultAsync(predicate, token);

            if(entity == null) throw new ArgumentException("Invalid identifier.");

            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Notes = dto.Notes;

            await _dataContext.SaveChangesAsync(token);

            return AsDto(entity);
        }

        public async Task<UserDto> UpdateAsync(Expression<Func<User, bool>> predicate, CreateUserDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Users.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentException("Invalid identifier.");

            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Notes = dto.Notes ?? entity.Notes;

            await _dataContext.SaveChangesAsync(token);
            return AsDto(entity);
        }
    }
}
