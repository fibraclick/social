using FibraClickSocial.Interfaces;

namespace FibraClickSocial.Configuration
{
    class TelegramConfiguration : IValidatable
    {
        /// <summary>
        /// Whether Telegram publishing is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Telegram bot token. Get one with @BotFather on Telegram
        /// </summary>
        public string BotToken { get; set; }

        /// <summary>
        /// Username of the Telegram channel where messages will be sent
        /// </summary>
        public string ChannelUsername { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.BotToken))
            {
                throw new SettingsValidationException(nameof(TelegramConfiguration), nameof(this.BotToken), "must be a non-empty string");
            }

            if (string.IsNullOrEmpty(this.ChannelUsername))
            {
                throw new SettingsValidationException(nameof(TelegramConfiguration), nameof(this.ChannelUsername), "must be a non-empty string");
            }
        }
    }
}
