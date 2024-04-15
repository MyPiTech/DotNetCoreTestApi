// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 03-11-2024
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 04-11-2024
// ***********************************************************************
// <copyright file="HealthController.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Test.Data;
using TestApi.Hubs;

namespace TestApi.Controllers
{
	/// <summary>
	/// Class HealthController.
	/// Implements the <see cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.HealthController}" />
	/// </summary>
	/// <seealso cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.HealthController}" />
	[ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HealthController : ApiControllerBase<HealthController>
    {
		/// <summary>
		/// The data context
		/// </summary>
		private readonly MSTestDataContext _dataContext;
		/// <summary>
		/// The memory cache
		/// </summary>
		private readonly IMemoryCache _memoryCache;
		/// <summary>
		/// The hc key
		/// </summary>
		private const string HC_KEY = "healthCheck";
		/// <summary>
		/// Initializes a new instance of the <see cref="HealthController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="dataContext">The data context.</param>
		/// <param name="memoryCache">The memory cache.</param>
		/// <param name="consoleHub">The console hub.</param>
		/// <param name="accessor">The accessor.</param>
		public HealthController(
			ILogger<HealthController> logger, 
			MSTestDataContext dataContext, 
			IMemoryCache memoryCache,
			IHubContext<ConsoleHub, IConsoleHub> consoleHub,
			IHttpContextAccessor accessor
			) : base(logger, consoleHub, accessor)
        {
			_dataContext = dataContext;
			_memoryCache = memoryCache;
		}

		/// <summary>
		/// Check as an asynchronous operation.
		/// </summary>
		/// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
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

				var expirationTime = DateTimeOffset.Now.AddMinutes(30);
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
