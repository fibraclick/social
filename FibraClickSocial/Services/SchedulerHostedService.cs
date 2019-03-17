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

        private readonly CultureInfo culture = new CultureInfo("it-IT");

        public SchedulerHostedService(IWholesaleService wholesale,
                                      ILogger<SchedulerHostedService> logger,
                                      ITelegramService telegram)
        {
            this.wholesale = wholesale;
            this.logger = logger;
            this.telegram = telegram;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RunCheck();
        }

        private async Task RunCheck()
        {
            DateTimeOffset currentVersion = await this.wholesale.GetCurrentVersion();

            this.logger.LogInformation("Wholesale current version is {Version}", currentVersion.ToString(culture));

            DateTimeOffset previousVersion;

            // Read last version unix timestamp
            if (File.Exists("version.txt"))
            {
                string cache = await File.ReadAllTextAsync("version.txt");
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

                await File.WriteAllTextAsync("version.txt", unixTs);

                // Avoids publishing the first time
                previousVersion = currentVersion;
            }

            if (currentVersion != previousVersion)
            {
                this.logger.LogInformation("Versions are different");

                string date = currentVersion.ToString("dd MMMM yyyy");

                this.logger.LogInformation("Publishing on Telegram...");

                await PublishTelegram(date);

                this.logger.LogInformation("Done");
            }
        }

        private Task PublishTelegram(string date)
        {
            string message = string.Format(MessageTemplates.Telegram, date);

            return this.telegram.SendMessage(message);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
