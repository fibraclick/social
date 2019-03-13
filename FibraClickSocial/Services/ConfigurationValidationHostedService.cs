using FibraClickSocial.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class ConfigurationValidationHostedService : IHostedService
    {
        public ConfigurationValidationHostedService(IOptions<WholesaleConfiguration> w,
                                                    IOptions<TelegramConfiguration> tg,
                                                    IOptions<TwitterConfiguration> tw,
                                                    IOptions<FacebookConfiguration> fb)
        {
            w.Value.Validate();
            tg.Value.Validate();
            tw.Value.Validate();
            fb.Value.Validate();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
