// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 12-31-2023
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 04-15-2024
// ***********************************************************************
// <copyright file="EventsController.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Test.Data;
using TestApi.Dtos;
using TestApi.Extensions;
using TestApi.Hubs;
using TestApi.Services;

namespace TestApi.Controllers
{
	/// <summary>
	/// Class EventsController.
	/// Implements the <see cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.EventsController}" />
	/// Implements the <see cref="TestApi.Controllers.ICrudController{TestApi.Dtos.CreateEventDto, TestApi.Dtos.EventDto}" />
	/// </summary>
	/// <seealso cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.EventsController}" />
	/// <seealso cref="TestApi.Controllers.ICrudController{TestApi.Dtos.CreateEventDto, TestApi.Dtos.EventDto}" />
	[ApiController]
    [Route("[controller]")]
    public class EventsController : ApiControllerBase<EventsController>, ICrudController<CreateEventDto, EventDto>
    {

		/// <summary>
		/// The service
		/// </summary>
		private readonly IService<Event, CreateEventDto, EventDto> _service;
		/// <summary>
		/// Initializes a new instance of the <see cref="EventsController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="service">The service.</param>
		/// <param name="consoleHub">The console hub.</param>
		/// <param name="accessor">The accessor.</param>
		public EventsController(
            ILogger<EventsController> logger, 
            IService<Event, CreateEventDto, EventDto> service,
			IHubContext<ConsoleHub, IConsoleHub> consoleHub,
			IHttpContextAccessor accessor
		) : base(logger, consoleHub, accessor)
        {
            _service = service;
        }

		/// <summary>
		/// Gets all events.
		/// </summary>
		/// <param name="token">The cancellation token.</param>
		/// <returns>All the event dtos.</returns>
		/// <response code="200">No errors occurred. Events returned.</response>
		/// <response code="404">No event found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IList<EventDto>>> GetAllAsync(CancellationToken token)
        {
			try
			{
				List<EventDto> dtos = await _service.GetAllAsync(null, token);
				await _logger.LogDebugAsync("EventsController\\GetAllAsync", dtos);
				if (dtos.Count == 0)
				{
					var response = NoneFoundResult;
					await _logger.LogWarningAsync("EventsController\\GetAllAsync", response);
					return response;
				}

				return Ok(dtos);
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "EventsController\\GetAllAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Get an event.
		/// </summary>
		/// <param name="id">The event id.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The event dto.</returns>
		/// <response code="200">No errors occurred. Event returned.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> GetAsync(int id, CancellationToken token)
        {
			try
			{
				var returnDto = await _service.GetAsync(i => i.Id == id, token);
				if (returnDto == null)
				{
					var response = NotFoundResult(id);
					await _logger.LogWarningAsync("EventsController\\GetAsync", response);
					return response;
				}

				await _logger.LogDebugAsync("EventsController\\GetAsync", returnDto);
				return Ok(returnDto);
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "EventsController\\GetAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Creates a new event.
		/// </summary>
		/// <param name="dto">The create event dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The new event dto.</returns>
		/// <response code="201">User created.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> CreateAsync(CreateEventDto dto, CancellationToken token)
        {
			try
			{
				if (!ModelState.IsValid)
				{
					var response = BadRequest(ModelState);
					await _logger.LogWarningAsync("EventsController\\CreateAsync", response);
					return response;
				}

				var returnDto = await _service.CreateAsync(dto, token);
				var result = CreatedAtAction("Create", new { id = returnDto.Id }, returnDto);
				await _logger.LogInformationAsync("EventsController\\CreateAsync", result);
				return result;
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "EventsController\\CreateAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Delete an event.
		/// </summary>
		/// <param name="id">The event id.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No event found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken token )
		{
			try
			{
				await _service.DeleteAsync(i => i.Id == id, token);
				await _logger.LogInformationAsync("EventsController\\DeleteAsync - id:", id);
				return NoContent();
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "EventsController\\DeleteAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Replace an event.
		/// </summary>
		/// <param name="id">The event id.</param>
		/// <param name="dto">The create event dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The event dto.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No event found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> ReplaceAsync(int id, CreateEventDto dto, CancellationToken token)
        {
			try
			{
				if (!ModelState.IsValid)
				{
					var response = BadRequest(ModelState);
					await _logger.LogWarningAsync("EventsController\\ReplaceAsync", response);
					return response;
				}
				var result = await _service.ReplaceAsync(i => i.Id == id, dto, token);
				await _logger.LogInformationAsync("EventsController\\ReplaceAsync", result);
				return Ok(result);
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "EventsController\\ReplaceAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Update an event.
		/// </summary>
		/// <param name="id">The event id.</param>
		/// <param name="dto">The create event dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The updated event dto.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No event found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> UpdateAsync(int id, CreateEventDto dto, CancellationToken token)
        {
			try
			{
				if (!ModelState.IsValid)
				{
					var response = BadRequest(ModelState);
					await _logger.LogWarningAsync("EventsController\\UpdateAsync", response);
					return response;
				}
				var result = await _service.UpdateAsync(i => i.Id == id, dto, token);
				await _logger.LogInformationAsync("EventsController\\UpdateAsync", result);
				return Ok(result);
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "EventsController\\UpdateAsync");
				return BadRequest(ex.Message);
			}
        }
    }
}
