namespace FibraClickSocial.Configuration
{
    class TelegramConfiguration : IValidatable
    {
        /// <summary>
        /// Telegram bot token. Get one with @BotFather on Telegram
        /// </summary>
        public string BotToken { get; set; }

        /// <summary>
        /// Username of the Telegram channel where messages will be sent
        /// </summary>
        public string Channel { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.BotToken))
            {
                throw new SettingsValidationException(nameof(TelegramConfiguration), nameof(this.BotToken), "must be a non-empty string");
            }
        }
    }
}
