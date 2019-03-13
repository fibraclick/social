using System;

namespace FibraClickSocial.Configuration
{
    class WholesaleConfiguration : IValidatable
    {
        public Uri Url { get; set; }

        public void Validate()
        {
            if (this.Url == null)
            {
                throw new SettingsValidationException(nameof(WholesaleConfiguration), nameof(this.Url), "must be a valid Uri");
            }
        }
    }
}
