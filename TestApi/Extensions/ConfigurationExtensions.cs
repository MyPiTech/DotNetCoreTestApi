using TestApi.Settings;

namespace TestApi.Extensions
{
    public static class  ConfigurationExtensions
    {
        /// <summary>Gets the email setting.</summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static string? GetEmailSetting(this IConfiguration configuration, string name)
        {
            return configuration?.GetSection("EmailSettings")[name];
        }

        /// <summary>Gets the email settings.</summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static EmailSettings? GetEmailSettings(this IConfiguration configuration)
        {
            return configuration?.GetSection("EmailSettings").Get<EmailSettings>();
        }
    }
}
