// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 04-05-2024
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 04-15-2024
// ***********************************************************************
// <copyright file="ConsoleHub.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.SignalR;

namespace TestApi.Hubs
{
	/// <summary>
	/// Class ConsoleHub.
	/// Implements the <see cref="Microsoft.AspNetCore.SignalR.Hub{TestApi.Hubs.IConsoleHub}" />
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.SignalR.Hub{TestApi.Hubs.IConsoleHub}" />
	public class ConsoleHub : Hub<IConsoleHub>
    {

		/// <summary>
		/// Send API log as an asynchronous operation.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public async Task SendApiLogAsync(object? data)
        {
            await Clients.All.SendApiLogAsync(data ?? string.Empty);
        }
		/// <summary>
		/// Send API information as an asynchronous operation.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public async Task SendApiInfoAsync(object? data)
        {
            await Clients.All.SendApiInfoAsync(data ?? string.Empty);
        }
		/// <summary>
		/// Send API warn as an asynchronous operation.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public async Task SendApiWarnAsync(object? data)
        {
            await Clients.All.SendApiWarnAsync(data ?? string.Empty);
        }
		/// <summary>
		/// Send API error as an asynchronous operation.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public async Task SendApiErrorAsync(object? data)
        {
            await Clients.All.SendApiErrorAsync(data ?? string.Empty);
        }
	}
}
