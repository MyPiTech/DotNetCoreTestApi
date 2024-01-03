namespace TestApi.Services
{
    //Simple interface to demonstrate polymorphism and generics.
    public interface IService<C, R>
        where C : class
        where R : class
    {
        Task<List<R>> GetAllAsync(CancellationToken token);

        Task<R?> GetAsync(int id, CancellationToken token);

        Task<R> CreateAsync(C createDto, CancellationToken token);

        Task<bool> DeleteAsync(int id, CancellationToken token);

        Task<R?> ReplaceAsync(int id, C createDto, CancellationToken token);

        Task<R?> UpdateAsync(int id, R createDto, CancellationToken token);
    }
}
