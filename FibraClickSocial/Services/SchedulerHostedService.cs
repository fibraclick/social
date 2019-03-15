using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class SchedulerHostedService : IHostedService
    {
        public SchedulerHostedService()
        {

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
