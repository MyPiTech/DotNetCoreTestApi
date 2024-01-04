using System.Linq.Expressions;
using Test.Data;

namespace TestApi.Services
{
    //Simple interface to demonstrate polymorphism and generics.
    public interface IService<E, C, R>
        where E : class
        where C : class
        where R : class
    {
        Task<List<R>> GetAllAsync(Expression<Func<E, bool>>? predicate, CancellationToken token);

        Task<R?> GetAsync(Expression<Func<E, bool>> predicate, CancellationToken token);

        Task<R> CreateAsync(C dto, CancellationToken token, int? parentId = null);

        Task DeleteAsync(Expression<Func<E, bool>> predicate, CancellationToken token);

        Task<R> ReplaceAsync(Expression<Func<E, bool>> predicate, C dto, CancellationToken token);

        Task<R> UpdateAsync(Expression<Func<E, bool>> predicate, C dto, CancellationToken token);
    }
}
