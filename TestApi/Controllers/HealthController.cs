
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Test.Data;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HealthController : ApiControllerBase<HealthController>
    {
		private readonly MSTestDataContext _dataContext;
		private readonly IMemoryCache _memoryCache;
		private const string HC_KEY = "healthCheck";
		public HealthController(ILogger<HealthController> logger, MSTestDataContext dataContext, IMemoryCache memoryCache) : base(logger)
        {
			_dataContext = dataContext;
			_memoryCache = memoryCache;
		}

        [HttpGet]
		[ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Any)]

		public async Task<ActionResult> CheckAsync(CancellationToken token)
		{
            try {
				var cachedResult = _memoryCache.Get<int?>(HC_KEY);
				if (cachedResult != null)
				{
					return Ok(cachedResult);
				}

				var expirationTime = DateTimeOffset.Now.AddMinutes(55);
				int result = await _dataContext.Database.ExecuteSqlRawAsync("SELECT 1;", token);
				_memoryCache.Set(HC_KEY, result, expirationTime);

				return Ok(result);

			} catch(Exception e) {
				_logger.LogError(e.Message);
				return BadRequest(e.Message);
			}	
		}

    }
}
