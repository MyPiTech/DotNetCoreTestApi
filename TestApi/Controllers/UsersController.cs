// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 12-29-2023
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 01-05-2024
// ***********************************************************************
// <copyright file="UsersController.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using TestApi.Extensions;
using TestApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Test.Data;
using TestApi.Dtos;
using TestApi.Services;

namespace TestApi.Controllers
{
	/// <summary>
	/// Class UsersController.
	/// Implements the <see cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.UsersController}" />
	/// Implements the <see cref="TestApi.Controllers.ICrudController{TestApi.Dtos.CreateUserDto, TestApi.Dtos.UserDto}" />
	/// </summary>
	/// <seealso cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.UsersController}" />
	/// <seealso cref="TestApi.Controllers.ICrudController{TestApi.Dtos.CreateUserDto, TestApi.Dtos.UserDto}" />
	[ApiController]
    [Route("[controller]")]
    public partial class UsersController : ApiControllerBase<UsersController>, ICrudController<CreateUserDto, UserDto>
    {
		/// <summary>
		/// The user service
		/// </summary>
		private readonly IService<User, CreateUserDto, UserDto> _userService;
		/// <summary>
		/// The event service
		/// </summary>
		private readonly IService<Event, CreateUserEventDto, EventDto> _eventService;

        private readonly IHubContext<ConsoleHub, IConsoleHub> _consoleHub;

		/// <summary>
		/// Initializes a new instance of the <see cref="UsersController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="userService">The user service.</param>
		/// <param name="eventService">The event service.</param>
		/// <param name="consoleHub"></param>
		public UsersController(
            ILogger<UsersController> logger, 
            IService<User, CreateUserDto, UserDto> userService, 
            IService<Event, CreateUserEventDto, EventDto> eventService, 
            IHubContext<ConsoleHub, IConsoleHub> consoleHub) : base(logger)
        {
            _userService = userService;
            _eventService = eventService;
            _consoleHub = consoleHub;
        }

		/// <summary>
		/// Gets all users.
		/// </summary>
		/// <param name="token">The cancellation token.</param>
		/// <returns>All the user dtos.</returns>
		/// <response code="200">No errors occurred. Users returned.</response>
		/// <response code="404">No users found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IList<UserDto>>> GetAllAsync(CancellationToken token)
        {
            try
            {
                List<UserDto> dtos = await _userService.GetAllAsync(null, token);
				await _logger.LogDebugAsync("UsersController\\GetAllAsync", _consoleHub, dtos);
				if (dtos.Count == 0)
				{
					var response = NoneFoundResult;
					await _logger.LogWarningAsync("UsersController\\GetAllAsync", _consoleHub, response);
					return response;
				}

				return dtos;
            }
            catch (Exception ex)
            {
				await _logger.LogErrorAsync(ex, "UsersController\\GetAllAsync", _consoleHub);
				return BadRequest(ex.Message);
            }
        }

		/// <summary>
		/// Get a user.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The user dto.</returns>
		/// <response code="200">No errors occurred. User returned.</response>
		/// <response code="404">No user found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> GetAsync(int id, CancellationToken token)
        {
            try
            {
                var returnDto = await _userService.GetAsync(u => u.Id == id, token);
                if (returnDto == null)
				{
					var response = NotFoundResult(id);
					await _logger.LogWarningAsync("UsersController\\GetAsync", _consoleHub, response);
					return response;
				}

				await _logger.LogDebugAsync("UsersController\\GetAsync", _consoleHub, returnDto);
                return returnDto;
            }
            catch (Exception ex)
            {
				await _logger.LogErrorAsync(ex, "UsersController\\GetAsync", _consoleHub);
				return BadRequest(ex.Message);
            }
        }

		/// <summary>
		/// Creates a new user.
		/// </summary>
		/// <param name="dto">The create user dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The new user dto.</returns>
		/// <response code="201">User created.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken token)
        {
            try 
            {
                if (!ModelState.IsValid)
                {
                    var response = BadRequest(ModelState);
                    await _logger.LogWarningAsync("UsersController\\CreateAsync", _consoleHub, response);
                    return response;
                }

                var returnDto = await _userService.CreateAsync(dto, token);
                var result = CreatedAtAction("Create", new { id = returnDto.Id }, returnDto);
                await _logger.LogInformationAsync("UsersController\\CreateAsync", _consoleHub, result);
                return result;
            }
            catch(Exception ex)
            {
                await _logger.LogErrorAsync(ex, "UsersController\\CreateAsync", _consoleHub);
                return BadRequest(ex.Message);
            }
        }



		/// <summary>
		/// Delete a user.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No user found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken token)
        {
            try {
                await _userService.DeleteAsync(u => u.Id == id, token);
				await _logger.LogInformationAsync("UsersController\\DeleteAsync - id:", _consoleHub, id);
				return NoContent();
            }
            catch(Exception ex)
            {
				await _logger.LogErrorAsync(ex, "UsersController\\DeleteAsync", _consoleHub);
				return BadRequest(ex.Message);
            }
        }

		/// <summary>
		/// Replace a user.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="dto">The create user dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The user dto.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No user found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> ReplaceAsync(int id, CreateUserDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid)
				{
					var response = BadRequest(ModelState);
					await _logger.LogWarningAsync("UsersController\\ReplaceAsync", _consoleHub, response);
					return response;
				}
				var result = await _userService.ReplaceAsync(u => u.Id == id, dto, token);
				await _logger.LogInformationAsync("UsersController\\ReplaceAsync", _consoleHub, result);
				return result;
			}
            catch (Exception ex)
            {
				await _logger.LogErrorAsync(ex, "UsersController\\ReplaceAsync", _consoleHub);
				return BadRequest(ex.Message);
            }
        }

		/// <summary>
		/// Update a user.
		/// </summary>
		/// <param name="id">The user id.</param>
		/// <param name="dto">The create user dto.</param>
		/// <param name="token">The cancellation token.</param>
		/// <returns>The user dto.</returns>
		/// <response code="204">No errors occurred.</response>
		/// <response code="404">No user found.</response>
		/// <response code="400">Unanticipated error occurred.</response>
		[HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> UpdateAsync(int id, CreateUserDto dto, CancellationToken token)
        {
            try
            {
                if (!ModelState.IsValid)
				{
					var response = BadRequest(ModelState);
					await _logger.LogWarningAsync("UsersController\\UpdateAsync", _consoleHub, response);
					return response;
				}
				var result = await _userService.UpdateAsync(u => u.Id == id, dto, token);
				await _logger.LogInformationAsync("UsersController\\UpdateAsync", _consoleHub, result);
				return result;
            }
            catch (Exception ex)
            {
				await _logger.LogErrorAsync(ex, "UsersController\\UpdateAsync", _consoleHub);
				return BadRequest(ex.Message);
            }
        }
    }
}