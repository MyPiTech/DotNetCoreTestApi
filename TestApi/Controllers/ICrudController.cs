using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    //Simple interface to demonstrate polymorphism and generics.
    public interface ICrudController<C, R>
        where C : class
        where R : class
    {

        Task<ActionResult<IList<R>>> GetAllAsync(CancellationToken token);

        Task<ActionResult<R>> GetAsync(int id, CancellationToken token);

        Task<ActionResult<R>> CreateAsync(C create, CancellationToken token);

        Task<ActionResult> DeleteAsync(int id, CancellationToken token);

        Task<ActionResult<R>> ReplaceAsync(int id, C replace, CancellationToken token);

        Task<ActionResult<R>> UpdateAsync(int id, C update, CancellationToken token);
    }
}
