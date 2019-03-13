namespace FibraClickSocial.Configuration
{
    class TwitterConfiguration : IValidatable
    {
        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string AccessToken { get; set; }

        public string AccessTokenSecret { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.ConsumerKey))
            {
                throw new SettingsValidationException(nameof(TwitterConfiguration), nameof(this.ConsumerKey), "must be a non-empty string");
            }

            if (string.IsNullOrEmpty(this.ConsumerSecret))
            {
                throw new SettingsValidationException(nameof(TwitterConfiguration), nameof(this.ConsumerSecret), "must be a non-empty string");
            }

            if (string.IsNullOrEmpty(this.AccessToken))
            {
                throw new SettingsValidationException(nameof(TwitterConfiguration), nameof(this.AccessToken), "must be a non-empty string");
            }

            if (string.IsNullOrEmpty(this.AccessTokenSecret))
            {
                throw new SettingsValidationException(nameof(TwitterConfiguration), nameof(this.AccessTokenSecret), "must be a non-empty string");
            }
        }
    }
}
