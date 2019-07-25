using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class SocialService : ISocialService
    {
        private readonly ITelegramService telegram;
        private readonly IFacebookService facebook;
        private readonly ITwitterService twitter;
        private readonly ILogger<SocialService> logger;

        public SocialService(ITelegramService telegram,
                             IFacebookService facebook,
                             ITwitterService twitter,
                             ILogger<SocialService> logger)
        {
            this.telegram = telegram;
            this.facebook = facebook;
            this.twitter = twitter;
            this.logger = logger;
        }

        public async Task Publish(string telegram, string twitter, string facebook)
        {
            if (this.telegram.Enabled)
            {
                this.logger.LogInformation("Publishing to Telegram...");

                await this.SendSingle(this.telegram, telegram);
            }

            if (this.twitter.Enabled)
            {
                this.logger.LogInformation("Publishing to Twitter...");

                await this.SendSingle(this.twitter, twitter);
            }

            if (this.facebook.Enabled)
            {
                this.logger.LogInformation("Publishing to Facebook...");
                await this.SendSingle(this.facebook, facebook);
            }
        }

        private async Task SendSingle(ISendMessage social, string message)
        {
            try
            {
                await social.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while publishing. Going on...");
            }
        }
    }
}
