// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 12-31-2023
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 04-15-2024
// ***********************************************************************
// <copyright file="ApiControllerBase.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TestApi.Extensions;
using TestApi.Hubs;

namespace TestApi.Controllers
{
	//Simple base class to demonstrate inheritance and generics.
	/// <summary>
	/// Class ApiControllerBase.
	/// Implements the <see cref="ControllerBase" />
	/// </summary>
	/// <typeparam name="C"></typeparam>
	/// <seealso cref="ControllerBase" />
	public class ApiControllerBase<C> : ControllerBase where C : class
    {
		/// <summary>
		/// The logger
		/// </summary>
		protected readonly ILogger<C> _logger;
		/// <summary>
		/// Initializes a new instance of the <see cref="ApiControllerBase{C}" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="consoleHub">The console hub.</param>
		/// <param name="accessor">The accessor.</param>
		public ApiControllerBase(
            ILogger<C> logger,
			IHubContext<ConsoleHub, IConsoleHub> consoleHub,
			IHttpContextAccessor accessor
        )
        {
            _logger = logger;
			_logger.InitHub(consoleHub);
		}


		/// <summary>
		/// Simple example of the DRY principle.
		/// Gets the not found result for single item
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>ActionResult.</returns>
		protected ActionResult NotFoundResult(int id) => NotFound($"Id: {id} was not found.");

		/// <summary>
		/// Gets the none found result.
		/// </summary>
		/// <value>The none found result.</value>
		protected ActionResult NoneFoundResult => NotFound($"No records were found.");
    }
}
