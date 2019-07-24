using Microsoft.Extensions.Logging;
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
                await this.telegram.SendMessageAsync(telegram);
            }

            if (this.twitter.Enabled)
            {
                this.logger.LogInformation("Publishing to Twitter...");
                await this.twitter.SendMessageAsync(twitter);
            }

            if (this.facebook.Enabled)
            {
                this.logger.LogInformation("Publishing to Facebook...");
                await this.facebook.SendMessageAsync(facebook);
            }
        }
    }
}
