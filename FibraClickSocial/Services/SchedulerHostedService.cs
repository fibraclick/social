using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FibraClickSocial.Interfaces;

namespace FibraClickSocial.Services
{
    class SchedulerHostedService : IHostedService
    {
        private readonly IWholesaleService wholesale;
        private readonly IFlashFiberService flashfiber;
        private readonly ILogger<SchedulerHostedService> logger;
        private readonly ISocialService social;

        private readonly CultureInfo culture = new CultureInfo("it-IT");
        private readonly TimeSpan CHECK_INTERVAL = TimeSpan.FromMinutes(5);

        public SchedulerHostedService(IWholesaleService wholesale,
                                      IFlashFiberService flashfiber,
                                      ILogger<SchedulerHostedService> logger,
                                      ISocialService social)
        {
            this.wholesale = wholesale;
            this.flashfiber = flashfiber;
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
            await CheckWholesale();
            await CheckFlashFiber();
        }

        private async Task CheckWholesale()
        {
            DateTimeOffset currentVersion = await this.wholesale.GetCurrentVersion();

            if (currentVersion == default)
            {
                this.logger.LogWarning("[Wholesale] Couldn't get current version");
                return;
            }

            this.logger.LogInformation("[Wholesale] Current version is {Version}", currentVersion.ToString(culture));

            DateTimeOffset previousVersion = await this.wholesale.GetPreviousVersion(currentVersion);

            this.logger.LogInformation("[Wholesale] Previous version is {Version}", previousVersion.ToString(culture));

            if (currentVersion != previousVersion)
            {
                await this.wholesale.UpdateCurrentVersion(currentVersion);

                this.logger.LogInformation("[Wholesale] Versions are different");

                string date = currentVersion.ToString("d MMMM yyyy", culture);

                await this.social.Publish(
                    telegram: string.Format(MessageTemplates.Wholesale.Telegram, date),
                    twitter: string.Format(MessageTemplates.Wholesale.Twitter, date),
                    facebook: string.Format(MessageTemplates.Wholesale.Facebook, date)
                );

                this.logger.LogInformation("[Wholesale] Done");
            }
        }

        private async Task CheckFlashFiber()
        {
            string currentVersion = await this.flashfiber.GetCurrentVersion();

            if (currentVersion == null)
            {
                this.logger.LogWarning("[FlashFiber] Couldn't get current version");
                return;
            }

            this.logger.LogInformation("[FlashFiber] Current version is {Version}", currentVersion);

            string previousVersion = await this.flashfiber.GetPreviousVersion(currentVersion);

            this.logger.LogInformation("[FlashFiber] Previous version is {Version}", previousVersion);

            if (currentVersion != previousVersion)
            {
                await this.flashfiber.UpdateCurrentVersion(currentVersion);

                this.logger.LogInformation("[FlashFiber] Versions are different");

                await this.social.Publish(
                    telegram: string.Format(MessageTemplates.FlashFiber.Telegram, currentVersion),
                    twitter: string.Format(MessageTemplates.FlashFiber.Twitter, currentVersion),
                    facebook: string.Format(MessageTemplates.FlashFiber.Facebook, currentVersion)
                );

                this.logger.LogInformation("[FlashFiber] Done");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
