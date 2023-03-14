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
        private readonly IFiberCopService fibercop;
        private readonly ILogger<SchedulerHostedService> logger;
        private readonly ISocialService social;

        private readonly CultureInfo culture = new CultureInfo("it-IT");
        private readonly TimeSpan CHECK_INTERVAL = TimeSpan.FromMinutes(5);

        public SchedulerHostedService(IWholesaleService wholesale,
            IFiberCopService fibercop,
            ILogger<SchedulerHostedService> logger,
            ISocialService social)
        {
            this.wholesale = wholesale;
            this.fibercop = fibercop;
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
            // await CheckWholesale();
            await CheckFiberCop();
        }

        private async Task CheckWholesale()
        {
            DateTimeOffset currentVersion;

            try
            {
                currentVersion = await this.wholesale.GetCurrentVersion();
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "[Wholesale] Couldn't get current version");
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

        private async Task CheckFiberCop()
        {
            string currentCount;

            try
            {
                currentCount = await this.fibercop.GetCurrentCount();
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "[FiberCop] Couldn't get current count");
                return;
            }

            this.logger.LogInformation("[FiberCop] Current count is {Version}", currentCount);

            string previousVersion = await this.fibercop.GetPreviousCount();

            this.logger.LogInformation("[FiberCop] Previous count is {Version}", previousVersion);

            if (previousVersion == default)
            {
                this.logger.LogWarning("[FiberCop] No previous count, updating cache with {Version}", currentCount);
                await this.fibercop.UpdateCurrentCount(currentCount);
            }
            else if (currentCount != previousVersion)
            {
                await this.fibercop.UpdateCurrentCount(currentCount);

                if (!this.fibercop.ShouldNotify())
                {
                    this.logger.LogWarning("[FiberCop] Duplicate avoided");
                    return;
                }

                this.fibercop.SetNotified();

                this.logger.LogInformation("[FiberCop] Counts are different");

                string date = DateTime.Now.ToString("d MMMM yyyy", culture);

                await this.social.Publish(
                    telegram: string.Format(MessageTemplates.FiberCop.Telegram, date),
                    twitter: string.Format(MessageTemplates.FiberCop.Twitter, date),
                    facebook: string.Format(MessageTemplates.FiberCop.Facebook, date)
                );

                this.logger.LogInformation("[FiberCop] Done");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
