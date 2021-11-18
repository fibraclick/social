using System;
using FibraClickSocial.Interfaces;

namespace FibraClickSocial.Configuration
{
    class FiberCopConfiguration : IValidatable
    {
        /// <summary>
        /// URL of the resource that should be monitored
        /// </summary>
        public Uri Url { get; set; }

        public void Validate()
        {
            if (this.Url == null)
            {
                throw new SettingsValidationException(nameof(FiberCopConfiguration), nameof(this.Url), "must be a valid Uri");
            }
        }
    }
}
