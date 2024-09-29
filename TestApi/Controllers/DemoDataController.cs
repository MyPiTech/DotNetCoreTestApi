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
	/// Implements the <see cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.DemoDataController}" />
	/// </summary>
	/// <seealso cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.DemoDataController}" />
	[ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DemoDataController : ApiControllerBase<DemoDataController>
    {
		/// <summary>
		/// The data context
		/// </summary>
		private readonly MSTestDataContext _dataContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="DemoDataController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="dataContext">The data context.</param>
		/// <param name="consoleHub">The console hub.</param>
		/// <param name="accessor">The accessor.</param>
		public DemoDataController(
			ILogger<DemoDataController> logger, 
			MSTestDataContext dataContext, 
			IHubContext<ConsoleHub, IConsoleHub> consoleHub,
			IHttpContextAccessor accessor
			) : base(logger, consoleHub, accessor)
        {
			_dataContext = dataContext;
		}


		[HttpGet]
		public async Task<ActionResult> AddDemoDataAsync(CancellationToken token)
		{
            try {
				

				if (!await _dataContext.Users.AnyAsync(u => u.FirstName == "Marty" && u.LastName == "McFly"))
				{
					var dance = new Event
					{
						Title = "The Enchantment Under The Sea Dance",
						Location = "Hill Valley High School",
						Duration = 90,
						Start = new DateTime(1955, 11, 12, 20, 0, 0)
					};

					var user = new User
					{
						FirstName = "Marty",
						LastName = "McFly",
						Notes = "Wait a minute. Wait a minute, Doc. Ah... Are you telling me that you built a time machine... out of a DeLorean?"
					};
					user.Events.Add(dance);
					await _dataContext.AddAsync(user, token);
					await _dataContext.SaveChangesAsync(token);
				};

				if (!await _dataContext.Users.AnyAsync(u => u.FirstName == "Emmett" && u.LastName == "Brown"))
				{
					var dance = new Event
					{
						Title = "The Enchantment Under The Sea Dance",
						Location = "Hill Valley High School",
						Duration = 90,
						Start = new DateTime(1955, 11, 12, 20, 0, 0)
					};

					var user = new User
					{
						FirstName = "Emmett",
						LastName = "Brown",
						Notes = "Oh, my God. They found me. I don't know how, but they found me. Run for it, Marty!"
					};
					user.Events.Add(dance);
					await _dataContext.AddAsync(user, token);
					await _dataContext.SaveChangesAsync(token);
				};

				if (!await _dataContext.Users.AnyAsync(u => u.FirstName == "Biff" && u.LastName == "Tannen"))
				{
					var dance = new Event
					{
						Title = "The Enchantment Under The Sea Dance",
						Location = "Hill Valley High School",
						Duration = 90,
						Start = new DateTime(1955, 11, 12, 20, 0, 0)
					};

					var user = new User
					{
						FirstName = "Biff",
						LastName = "Tannen",
						Notes = "Since you're new here, I-I'm gonna cut you a break, today. So, why don't you make like a tree and get outta here?"
					};
					user.Events.Add(dance);
					await _dataContext.AddAsync(user, token);
					await _dataContext.SaveChangesAsync(token);
				};

				if (!await _dataContext.Users.AnyAsync(u => u.FirstName == "Lorraine" && u.LastName == "Baines"))
				{
					var dance = new Event
					{
						Title = "The Enchantment Under The Sea Dance",
						Location = "Hill Valley High School",
						Duration = 90,
						Start = new DateTime(1955, 11, 12, 20, 0, 0)
					};

					var user = new User
					{
						FirstName = "Lorraine",
						LastName = "Baines",
						Notes = "This is all wrong. I don't know what it is. But when I kiss you, it's like I'm kissing... my brother. I guess that doesn't make any sense, does it?"
					};
					user.Events.Add(dance);
					await _dataContext.AddAsync(user, token);
					await _dataContext.SaveChangesAsync(token);
				};

				if (!await _dataContext.Users.AnyAsync(u => u.FirstName == "George" && u.LastName == "McFly"))
				{
					var dance = new Event
					{
						Title = "The Enchantment Under The Sea Dance",
						Location = "Hill Valley High School",
						Duration = 90,
						Start = new DateTime(1955, 11, 12, 20, 0, 0)
					};

					var user = new User
					{
						FirstName = "George",
						LastName = "McFly",
						Notes = "Last night, Darth Vader came down from Planet Vulcan and told me that if I didn't take Lorraine out, that he'd melt my brain."
					};
					user.Events.Add(dance);
					await _dataContext.AddAsync(user, token);
					await _dataContext.SaveChangesAsync(token);
				};

				if (!await _dataContext.Users.AnyAsync(u => u.FirstName == "Mr." && u.LastName == "Strickland"))
				{
					var dance = new Event
					{
						Title = "The Enchantment Under The Sea Dance",
						Location = "Hill Valley High School",
						Duration = 90,
						Start = new DateTime(1955, 11, 12, 20, 0, 0)
					};

					var user = new User
					{
						FirstName = "Mr.",
						LastName = "Strickland",
						Notes = "No McFly ever amounted to anything in the history of Hill Valley!"
					};
					user.Events.Add(dance);
					await _dataContext.AddAsync(user, token);
					await _dataContext.SaveChangesAsync(token);
				};

				return Ok();

			} catch(Exception e) {
				_logger.LogError(e.Message);
				return BadRequest(e.Message);
			}	
		}

	}
}
