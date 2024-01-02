using Microsoft.AspNetCore.Mvc;
using TestApi.Dtos;

namespace TestApi.Controllers
{
    //Simple interface to demonstrate polymorphism and generics.
    public interface ICrudController<C, R>
        where C : class
        where R : class
    {

        Task<ActionResult<R>> CreateAsync(C create);

        Task<ActionResult<IList<R>>> GetAllAsync();

        Task<ActionResult<R>> GetAsync(int id);

        Task<ActionResult> DeleteAsync(int id);

        Task<ActionResult<R>> ReplaceAsync(int id, C replace);

        Task<ActionResult<R>> UpdateAsync(int id, R update);
    }
}
