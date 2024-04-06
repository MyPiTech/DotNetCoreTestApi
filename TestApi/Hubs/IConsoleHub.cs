// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 04-05-2024
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 04-06-2024
// ***********************************************************************
// <copyright file="IConsoleHub.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace TestApi.Hubs
{
	/// <summary>
	/// Interface IConsoleHub
	/// </summary>
	public interface IConsoleHub
    {
		/// <summary>
		/// Sends the API log asynchronous.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>Task.</returns>
		Task SendApiLogAsync(params object[]? data);
		/// <summary>
		/// Sends the API information asynchronous.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>Task.</returns>
		Task SendApiInfoAsync(params object[]? data);
		/// <summary>
		/// Sends the API warn asynchronous.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>Task.</returns>
		Task SendApiWarnAsync(params object[]? data);
		/// <summary>
		/// Sends the API error asynchronous.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>Task.</returns>
		Task SendApiErrorAsync(params object[]? data);

    }
}
