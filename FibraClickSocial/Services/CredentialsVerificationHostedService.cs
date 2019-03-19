using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class CredentialsVerificationHostedService : IHostedService
    {
        private readonly ITwitterService twitter;
        private readonly ITelegramService telegram;
        private readonly IFacebookService facebook;
        private readonly ILogger<CredentialsVerificationHostedService> logger;

        public CredentialsVerificationHostedService(ITwitterService twitter,
                                                    ITelegramService telegram,
                                                    IFacebookService facebook,
                                                    ILogger<CredentialsVerificationHostedService> logger)
        {
            this.twitter = twitter;
            this.telegram = telegram;
            this.facebook = facebook;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string name;

            if (this.telegram.Enabled)
            {
                name = await this.telegram.VerifyCredentials();
                this.logger.LogInformation("Verified Telegram credentials for @{Handle}", name);
            }

            if (this.twitter.Enabled)
            {
                name = await this.twitter.VerifyCredentials();
                this.logger.LogInformation("Verified Twitter credentials for @{Handle}", name);
            }

            if (this.facebook.Enabled)
            {
                name = await this.facebook.VerifyCredentials();
                this.logger.LogInformation("Verified Facebook credentials for {Handle}", name);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
