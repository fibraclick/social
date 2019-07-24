using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class SchedulerHostedService : IHostedService
    {
        private readonly IWholesaleService wholesale;
        private readonly ILogger<SchedulerHostedService> logger;
        private readonly ISocialService social;

        private readonly CultureInfo culture = new CultureInfo("it-IT");
        private readonly TimeSpan CHECK_INTERVAL = TimeSpan.FromMinutes(5);

        public SchedulerHostedService(IWholesaleService wholesale,
                                      ILogger<SchedulerHostedService> logger,
                                      ISocialService social)
        {
            this.wholesale = wholesale;
            this.logger = logger;
            this.social = social;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var t = new System.Timers.Timer();
            t.Elapsed += Tick;
            t.Interval = CHECK_INTERVAL.TotalMilliseconds;
            t.Start();

            return RunCheck();
        }

        private async void Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            await RunCheck();
        }

        private async Task RunCheck()
        {
            DateTimeOffset currentVersion = await this.wholesale.GetCurrentVersion();

            if (currentVersion == default)
            {
                this.logger.LogWarning("Couldn't get wholesale current version");
                return;
            }

            this.logger.LogInformation("Wholesale current version is {Version}", currentVersion.ToString(culture));

            DateTimeOffset previousVersion = await this.wholesale.GetPreviousVersion(currentVersion);

            this.logger.LogInformation("Wholesale previous version is {Version}", previousVersion.ToString(culture));

            if (currentVersion != previousVersion)
            {
                await this.wholesale.UpdateCurrentVersion(currentVersion);

                this.logger.LogInformation("Versions are different");

                string date = currentVersion.ToString("d MMMM yyyy", culture);

                await this.social.Publish(
                    telegram: string.Format(MessageTemplates.Wholesale.Telegram, date),
                    twitter: string.Format(MessageTemplates.Wholesale.Twitter, date),
                    facebook: string.Format(MessageTemplates.Wholesale.Facebook, date)
                );

                this.logger.LogInformation("Done");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
