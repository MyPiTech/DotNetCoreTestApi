// ***********************************************************************
// Assembly         : TestApi
// Author           : Shawn Wheeler
// Created          : 02-05-2024
//
// Last Modified By : Shawn Wheeler
// Last Modified On : 04-11-2024
// ***********************************************************************
// <copyright file="EmailController.cs" company="TestApi">
//     Copyright (c) MyPiTech. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Net.Mail;
using TestApi.Dtos;
using TestApi.Extensions;
using TestApi.Hubs;

namespace TestApi.Controllers
{
	/// <summary>
	/// Class EmailController.
	/// Implements the <see cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.EmailController}" />
	/// </summary>
	/// <seealso cref="TestApi.Controllers.ApiControllerBase{TestApi.Controllers.EmailController}" />
	[ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EmailController : ApiControllerBase<EmailController>
    {
		/// <summary>
		/// The configuration
		/// </summary>
		private IConfiguration _configuration;
		/// <summary>
		/// Initializes a new instance of the <see cref="EmailController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="configuration">The configuration.</param>
		/// <param name="consoleHub">The console hub.</param>
		/// <param name="accessor">The accessor.</param>
		public EmailController(
            ILogger<EmailController> logger, 
            IConfiguration configuration,
			IHubContext<ConsoleHub, IConsoleHub> consoleHub,
			IHttpContextAccessor accessor
            ) : base(logger, consoleHub, accessor)
        {
            _configuration = configuration;
        }

		/// <summary>
		/// Sends the email.
		/// </summary>
		/// <param name="dto">The dto.</param>
		/// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>ActionResult.</returns>
		[HttpPost]
        public async Task<ActionResult> SendEmail(EmailDto dto, CancellationToken token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var emailSettings = _configuration.GetEmailSettings();
            if (emailSettings == null) return BadRequest();

            MailMessage message = new(emailSettings.AdminEmail, emailSettings.AdminEmail)
            {
                Subject = $"Contact Form - {dto.Subject ?? string.Empty}",
                Body = $"{dto.Name} - {dto.Email} <br/><br/>{dto.Message}",
                IsBodyHtml = true
            };

            try
            {
                using (var client = new SmtpClient(emailSettings.Host, emailSettings.Port))
                {
                    client.Credentials = new NetworkCredential(emailSettings.AdminEmail, emailSettings.Password);
                    client.EnableSsl = true;

                    await client.SendMailAsync(message, token);
                }

   
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

    }
}
