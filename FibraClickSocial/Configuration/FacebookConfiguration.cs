namespace FibraClickSocial.Configuration
{
    class FacebookConfiguration : IValidatable
    {
        public string AccessToken { get; set; }

        public string PageId { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.AccessToken))
            {
                throw new SettingsValidationException(nameof(FacebookConfiguration), nameof(this.AccessToken), "must be a non-empty string");
            }

            if (string.IsNullOrEmpty(this.PageId))
            {
                throw new SettingsValidationException(nameof(FacebookConfiguration), nameof(this.PageId), "must be a non-empty string");
            }
        }
    }
}
