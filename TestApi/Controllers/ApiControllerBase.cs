using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    //Simple base class to demonstrate inheritance and generics.
    public class ApiControllerBase<C> : ControllerBase where C : class
    {
        protected readonly ILogger<C> _logger;
        public ApiControllerBase(ILogger<C> logger)
        {
            _logger = logger;
        }

        /*
         Simple example of the DRY principle.
         Also creates a convenient point for future localization.
         */
        protected ActionResult NotFoundResult(int id) => NotFound($"Id: {id} was not found.");

        protected ActionResult NoneFoundResult => NotFound($"No records were found.");
    }
}
