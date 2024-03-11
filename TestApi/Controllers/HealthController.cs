
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Test.Data;
using TestApi.Dtos;
using TestApi.Extensions;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HealthController : ApiControllerBase<HealthController>
    {
		private readonly MSTestDataContext _dataContext;
		public HealthController(ILogger<HealthController> logger, MSTestDataContext dataContext) : base(logger)
        {
			_dataContext = dataContext;
		}

        [HttpGet]
        public async Task<ActionResult> CheckAsync(CancellationToken token)
		{
            try {

				int test = await _dataContext.Database.ExecuteSqlRawAsync("SELECT 1;", token);
				return Ok(test);

			} catch(Exception e) {
				_logger.LogError(e.Message);
				return BadRequest(e.Message);
			}	
		}

    }
}
