namespace FibraClickSocial.Configuration
{
    class FacebookConfiguration : IValidatable
    {
        /// <summary>
        /// Whether Facebook publishing is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Facebook application ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Facebook application secret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Page access token. How to get one:
        /// 
        /// https://stackoverflow.com/a/43570120/1633924
        /// 
        /// 1. Graph API Explorer:
        ///     Select your App from the top right dropdown menu
        ///     Select "Get User Access Token" from dropdown(right of access token field) and select needed permissions
        ///     Copy user access token
        /// 2. Access Token Debugger:
        ///     Paste copied token and press "Debug"
        ///     Press "Extend Access Token" and copy the generated long-lived user access token
        /// 3. Graph API Explorer:
        ///     Paste copied token into the "Access Token" field
        ///     Make a GET request with "PAGE_ID?fields=access_token"
        ///     Find the permanent page access token in the response(node "access_token")
        /// </summary>
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
