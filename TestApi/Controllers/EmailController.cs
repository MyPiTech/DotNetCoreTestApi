
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using TestApi.Dtos;
using TestApi.Extensions;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EmailController : ApiControllerBase<EmailController>
    {
        private IConfiguration _configuration;
        public EmailController(ILogger<EmailController> logger, IConfiguration configuration) : base(logger)
        {
            _configuration = configuration;
        }

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
