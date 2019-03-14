namespace FibraClickSocial.Configuration
{
    class FacebookConfiguration : IValidatable
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }

        // https://stackoverflow.com/a/43570120/1633924
        public string AccessToken { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.AppId))
            {
                throw new SettingsValidationException(nameof(FacebookConfiguration), nameof(this.AppId), "must be a non-empty string");
            }

            if (string.IsNullOrEmpty(this.AppSecret))
            {
                throw new SettingsValidationException(nameof(FacebookConfiguration), nameof(this.AppSecret), "must be a non-empty string");
            }

            if (string.IsNullOrEmpty(this.AccessToken))
            {
                throw new SettingsValidationException(nameof(FacebookConfiguration), nameof(this.AccessToken), "must be a non-empty string");
            }
        }
    }
}
