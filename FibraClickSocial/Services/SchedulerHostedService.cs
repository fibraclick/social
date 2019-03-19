using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class SchedulerHostedService : IHostedService
    {
        private readonly IWholesaleService wholesale;
        private readonly ILogger<SchedulerHostedService> logger;
        private readonly ITelegramService telegram;
        private readonly IFacebookService facebook;
        private readonly ITwitterService twitter;

        private readonly CultureInfo culture = new CultureInfo("it-IT");

        private const string FILE_PATH = "version.txt";
        private const double CHECK_INTERVAL = 5 * 60 * 1000; // 5 minutes

        public SchedulerHostedService(IWholesaleService wholesale,
                                      ILogger<SchedulerHostedService> logger,
                                      ITelegramService telegram,
                                      IFacebookService facebook,
                                      ITwitterService twitter)
        {
            this.wholesale = wholesale;
            this.logger = logger;
            this.telegram = telegram;
            this.facebook = facebook;
            this.twitter = twitter;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var t = new System.Timers.Timer();
            t.Elapsed += Tick;
            t.Interval = CHECK_INTERVAL;
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

            this.logger.LogInformation("Wholesale current version is {Version}", currentVersion.ToString(culture));

            DateTimeOffset previousVersion;

            // Read last version unix timestamp
            if (File.Exists(FILE_PATH))
            {
                string cache = await File.ReadAllTextAsync(FILE_PATH);
                previousVersion = DateTimeOffset.FromUnixTimeSeconds(long.Parse(cache));

                this.logger.LogInformation("Wholesale previous version is {Version}", previousVersion.ToString(culture));
            }
            // Save the current timestamp as the last timestamp
            else
            {
                string unixTs = currentVersion.ToUnixTimeSeconds().ToString();

                this.logger.LogInformation("Creating version file with version: {Version} => {Unix}",
                    currentVersion.ToString(culture),
                    unixTs);

                await File.WriteAllTextAsync(FILE_PATH, unixTs);

                // Avoids publishing the first time
                previousVersion = currentVersion;
            }

            if (currentVersion != previousVersion)
            {
                await File.WriteAllTextAsync(FILE_PATH, currentVersion.ToUnixTimeSeconds().ToString());

                this.logger.LogInformation("Versions are different");

                string date = currentVersion.ToString("dd MMMM yyyy", culture);

                if (this.telegram.Enabled)
                {
                    this.logger.LogInformation("Publishing to Telegram...");
                    await PublishTelegram(date);
                }

                if (this.twitter.Enabled)
                {
                    this.logger.LogInformation("Publishing to Twitter...");
                    await PublishTwitter(date);
                }

                if (this.facebook.Enabled)
                {
                    this.logger.LogInformation("Publishing to Facebook...");
                    await PublishFacebook(date);
                }

                this.logger.LogInformation("Done");
            }
        }

        private Task PublishTelegram(string date)
        {
            string message = string.Format(MessageTemplates.Telegram, date);

            return this.telegram.SendMessageAsync(message);
        }

        private Task PublishFacebook(string date)
        {
            string message = string.Format(MessageTemplates.Facebook, date);

            return this.facebook.SendMessageAsync(message);
        }

        private Task PublishTwitter(string date)
        {
            string message = string.Format(MessageTemplates.Twitter, date);

            return this.twitter.SendMessageAsync(message);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
