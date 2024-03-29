// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 01-04-2024
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 01-07-2024
// ***********************************************************************
// <copyright file="UserEventService.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test.Data;
using TestApi.Dtos;

namespace TestApi.Services
{
	/// <summary>
	/// Class UserEventService.
	/// Implements the <see cref="TestApi.Services.Service{TestApi.Services.UserEventService, Test.Data.Event, TestApi.Dtos.EventDto}" />
	/// Implements the <see cref="TestApi.Services.IService{Test.Data.Event, TestApi.Dtos.CreateUserEventDto, TestApi.Dtos.EventDto}" />
	/// </summary>
	/// <seealso cref="TestApi.Services.Service{TestApi.Services.UserEventService, Test.Data.Event, TestApi.Dtos.EventDto}" />
	/// <seealso cref="TestApi.Services.IService{Test.Data.Event, TestApi.Dtos.CreateUserEventDto, TestApi.Dtos.EventDto}" />
	public class UserEventService : Service<UserEventService, Event, EventDto>, IService<Event, CreateUserEventDto, EventDto>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="UserEventService"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="dataContext">The data context.</param>
		public UserEventService(ILogger<UserEventService> logger, MSTestDataContext dataContext) : base(logger, dataContext)
        {
            _toDto = e => new EventDto { Id = e.Id, Title = e.Title, Duration = e.Duration, Location = e.Location, Start = e.Start, UserId = e.UserId };
        }

		/// <summary>
		/// Create as an asynchronous operation.
		/// </summary>
		/// <param name="dto">The dto.</param>
		/// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <param name="parentId">The parent identifier.</param>
		/// <returns>A Task&lt;EventDto&gt; representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentNullException">parentId</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">parentId</exception>
		public async Task<EventDto> CreateAsync(CreateUserEventDto dto, CancellationToken token, int? parentId = null)
        {
            if(parentId == null) throw new ArgumentNullException(nameof(parentId));
            if (!await ValidateParentAsync<User>(u => u.Id == parentId.Value, token)) throw new ArgumentOutOfRangeException(nameof(parentId));

            var entity = new Event
            {
                UserId = parentId.Value,
                Title = dto.Title,
                Location = dto.Location,
                Start = dto.Start,
                Duration = dto.Duration
            };

            await _dataContext.AddAsync(entity);
            await _dataContext.SaveChangesAsync(token);

            _logger.LogInformation($"Event: {entity.Title} Id:{entity.Id} created.");
            return AsDto(entity);
        }

		/// <summary>
		/// Delete as an asynchronous operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Invalid identifier.</exception>
		public async Task DeleteAsync(Expression<Func<Event, bool>> predicate, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentOutOfRangeException("Invalid identifier.");

            _dataContext.Remove(entity);
            await _dataContext.SaveChangesAsync(token);
        }

		/// <summary>
		/// Get all as an asynchronous operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentNullException">predicate</exception>
		public async Task<List<EventDto>> GetAllAsync(Expression<Func<Event, bool>>? predicate, CancellationToken token)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await Dtos(predicate).ToListAsync(token);
        }

		/// <summary>
		/// Get as an asynchronous operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>A Task&lt;EventDto&gt; representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Invalid identifier.</exception>
		public async Task<EventDto?> GetAsync(Expression<Func<Event, bool>> predicate, CancellationToken token)
        {
            var dto = await Dtos(predicate).FirstOrDefaultAsync(token);
            if (dto == null) throw new ArgumentOutOfRangeException("Invalid identifier.");
            return dto;
        }

		/// <summary>
		/// Replace as an asynchronous operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="dto">The dto.</param>
		/// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>A Task&lt;EventDto&gt; representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Invalid identifier.</exception>
		public async Task<EventDto> ReplaceAsync(Expression<Func<Event, bool>> predicate, CreateUserEventDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);
            if (entity == null) throw new ArgumentOutOfRangeException("Invalid identifier.");

            entity.Start = dto.Start;
            entity.Duration = dto.Duration;
            entity.Location = dto.Location;
            entity.Title = dto.Title;

            await _dataContext.SaveChangesAsync(token);

            return AsDto(entity);
        }

		/// <summary>
		/// Update as an asynchronous operation.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="dto">The dto.</param>
		/// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>A Task&lt;EventDto&gt; representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Invalid identifier.</exception>
		public async Task<EventDto> UpdateAsync(Expression<Func<Event, bool>> predicate, CreateUserEventDto dto, CancellationToken token)
        {
            var entity = await _dataContext.Events.FirstOrDefaultAsync(predicate, token);

            if (entity == null) throw new ArgumentOutOfRangeException("Invalid identifier.");

            entity.Start = dto.Start;
            entity.Duration = dto.Duration;
            entity.Location = dto.Location ?? entity.Location;
            entity.Title = dto.Title ?? entity.Title;

            await _dataContext.SaveChangesAsync(token);

            return AsDto(entity);
        }

        
    }
}
