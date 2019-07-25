using System;

namespace FibraClickSocial.Configuration
{
    class FlashFiberConfiguration : IValidatable
    {
        /// <summary>
        /// URL of the resource that should be monitored
        /// </summary>
        public Uri Url { get; set; }

        public void Validate()
        {
            if (this.Url == null)
            {
                throw new SettingsValidationException(nameof(FlashFiberConfiguration), nameof(this.Url), "must be a valid Uri");
            }
        }
    }
}
