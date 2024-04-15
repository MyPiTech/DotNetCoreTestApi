// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 01-04-2024
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 01-07-2024
// ***********************************************************************
// <copyright file="UserEventsController.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Mvc;
using TestApi.Dtos;
using TestApi.Extensions;

namespace TestApi.Controllers
{
	/// <summary>
	/// Class UsersController.
	/// Implements the <see cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.UsersController}" />
	/// Implements the <see cref="TestApi.Controllers.ICrudController{TestApi.Dtos.CreateUserDto, TestApi.Dtos.UserDto}" />
	/// </summary>
	/// <seealso cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.UsersController}" />
	/// <seealso cref="TestApi.Controllers.ICrudController{TestApi.Dtos.CreateUserDto, TestApi.Dtos.UserDto}" />
	public partial class UsersController : ApiControllerBase<UsersController>, ICrudController<CreateUserDto, UserDto>
    {
		/// <summary>
		/// Gets all user events.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>All the event dtos.</returns>
		/// <response code="200">No errors occurred. User events returned.</response>
		/// <response code="404">No event found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpGet("{id}/Events")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IList<EventDto>>> GetAllAsync(int id, CancellationToken token)
        {
			try
			{
				List<EventDto> dtos = await _eventService.GetAllAsync(i => i.UserId == id, token);
				await _logger.LogDebugAsync("UserEventsController\\GetAllAsync", dtos);
				if (dtos.Count == 0)
				{
					var response = NoneFoundResult;
					await _logger.LogWarningAsync("UserEventsController\\GetAllAsync", response);
					return response;
				}

				return Ok(dtos);
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "UserEventsController\\GetAllAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Get a user event.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="eventId">The event id.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The event dto.</returns>
		/// <response code="200">No errors occurred. Event returned.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpGet("{id}/Events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> GetAsync(int id, int eventId, CancellationToken token)
        {
			try
			{
				var returnDto = await _eventService.GetAsync(i => i.UserId == id && i.Id == eventId, token);
				if (returnDto == null)
				{
					var response = NotFoundResult(id);
					await _logger.LogWarningAsync("UserEventsController\\GetAsync", response);
					return response;
				}

				await _logger.LogDebugAsync("UserEventsController\\GetAsync", returnDto);
				return Ok(returnDto);
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "UserEventsController\\GetAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Creates a new user event.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="dto">The create user event dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The new event dto.</returns>
		/// <response code="201">Event created.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPost("{id}/Events")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> CreateAsync(int id, CreateUserEventDto dto, CancellationToken token)
        {
			try
			{
				if (!ModelState.IsValid)
				{
					var response = BadRequest(ModelState);
					await _logger.LogWarningAsync("UserEventsController\\CreateAsync", response);
					return response;
				}

				var returnDto = await _eventService.CreateAsync(dto, token, id);
				var result = CreatedAtAction("Create", new { id = returnDto.Id }, returnDto);
				await _logger.LogInformationAsync("UserEventsController\\CreateAsync", result);
				return result;
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "UserEventsController\\CreateAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Delete a user event.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="eventId">The event id.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No event found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpDelete("{id}/Events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAsync(int id, int eventId, CancellationToken token)
        {
			try
			{
				await _eventService.DeleteAsync(i => i.UserId == id && i.Id == eventId, token);
				await _logger.LogInformationAsync("UserEventsController\\DeleteAsync - id:", id);
				return NoContent();
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "UserEventsController\\DeleteAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Replace a user event.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="eventId">The event id.</param>
		/// <param name="dto">The create user event dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The event dto.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No event found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPut("{id}/Events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> ReplaceAsync(int id, int eventId, CreateUserEventDto dto, CancellationToken token)
        {
			try
			{
				if (!ModelState.IsValid)
				{
					var response = BadRequest(ModelState);
					await _logger.LogWarningAsync("UserEventsController\\ReplaceAsync", response);
					return response;
				}
				var result = await _eventService.ReplaceAsync(i => i.UserId == id && i.Id == eventId, dto, token);
				await _logger.LogInformationAsync("UserEventsController\\ReplaceAsync", result);
				return Ok(result);
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "UserEventsController\\ReplaceAsync");
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// Update a user event.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="eventId">The event id.</param>
		/// <param name="dto">The create user event dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The updated event dto.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No event found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPatch("{id}/Events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<EventDto>> UpdateAsync(int id, int eventId, CreateUserEventDto dto, CancellationToken token)
        {
			try
			{
				if (!ModelState.IsValid)
				{
					var response = BadRequest(ModelState);
					await _logger.LogWarningAsync("UserEventsController\\UpdateAsync", response);
					return response;
				}
				var result = await _eventService.UpdateAsync(i => i.UserId == id && i.Id == eventId, dto, token);
				await _logger.LogInformationAsync("UserEventsController\\UpdateAsync", result);
				return Ok(result);
			}
			catch (Exception ex)
			{
				await _logger.LogErrorAsync(ex, "UserEventsController\\UpdateAsync");
				return BadRequest(ex.Message);
			}
        }
    }
}
