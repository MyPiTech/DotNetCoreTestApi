using TestApi.Dtos;

namespace TestApi.Services
{
    //Simple interface to demonstrate inversion of control, inheritance and generics.
    public interface IUserService : IService<CreateUserDto, UserDto>
    {
        Task<UserDto?> GetAsync(int userId, CancellationToken token, bool includeEvents = true);
    }
}
