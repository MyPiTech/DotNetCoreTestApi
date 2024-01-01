using Microsoft.AspNetCore.Mvc;
using PGTest.Data;

namespace TestApi.Controllers
{
    //Simple base class to demonstrate inheritance and generics.
    public class ApiControllerBase<C> : ControllerBase where C : class
    {
        protected readonly ILogger<C> _logger;
        protected readonly PGTestDataContext _dataContext;
        public ApiControllerBase(ILogger<C> logger, PGTestDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        /*
         Simple example of the DRY principle.
         Also creates a convenient point for future localization.
         */
        protected ActionResult NotFoundResult(int id) => NotFound($"Id: {id} was not found.");
    }
}
