namespace FibraClickSocial.Configuration
{
    class TwitterConfiguration : IValidatable
    {
        /// <summary>
        /// Twitter application key
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// Twitter application secret
        /// </summary>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// Access token for the user
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Secret token for the user
        /// </summary>
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
