namespace FibraClickSocial.Configuration
{
    class TelegramConfiguration : IValidatable
    {
        public string BotToken { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.BotToken))
            {
                throw new SettingsValidationException(nameof(TelegramConfiguration), nameof(this.BotToken), "must be a non-empty string");
            }
        }
    }
}
