using System;
using FibraClickSocial.Interfaces;

namespace FibraClickSocial.Configuration
{
    class WholesaleConfiguration : IValidatable
    {
        /// <summary>
        /// URL of the resource that should be monitored
        /// </summary>
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
