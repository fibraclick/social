using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FibraClickSocial.Configuration;
using FibraClickSocial.Interfaces;
using FibraClickSocial.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlainConsoleLogger;

namespace FibraClickSocial
{
    class Program
    {
        /// <summary>
        /// Holds the <see cref="CancellationToken"/> used
        /// for shutting down the WebHost
        /// </summary>
        private static CancellationTokenSource shutdownTokenSource =
            new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            IHost host = new HostBuilder()
                    .ConfigureAppConfiguration(ConfigureApp)
                    .ConfigureServices(ConfigureServices)
                    .ConfigureLogging(ConfigureLogging)
                    .UseConsoleLifetime(opts => opts.SuppressStatusMessages = true)
                    .Build();

            // Get the logger
            ILogger logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                await host.RunAsync(shutdownTokenSource.Token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");

                // Stop the WebHost, otherwise it might hang here
                shutdownTokenSource.Cancel();

                // Required to stop all the threads.
                // With "return 1", the process could actually stay online forever
                Environment.Exit(1);
            }
        }

        private static void ConfigureApp(HostBuilderContext hostContext, IConfigurationBuilder configApp)
        {
            // Load the application settings
            configApp.SetBasePath(Directory.GetCurrentDirectory());
            configApp.AddJsonFile("appsettings.json");
        }

        private static void ConfigureLogging(HostBuilderContext hostContext, ILoggingBuilder logging)
        {
            logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
            logging.AddPlainConsole();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.Configure<WholesaleConfiguration>(hostContext.Configuration.GetSection("Wholesale"));
            services.Configure<FiberCopConfiguration>(hostContext.Configuration.GetSection("FiberCop"));

            services.Configure<TelegramConfiguration>(hostContext.Configuration.GetSection("Telegram"));
            services.Configure<TwitterConfiguration>(hostContext.Configuration.GetSection("Twitter"));
            services.Configure<FacebookConfiguration>(hostContext.Configuration.GetSection("Facebook"));

            // These also register the service as a transient service
            services.AddHttpClient<ITwitterService, TwitterService>();
            services.AddHttpClient<ITelegramService, TelegramService>();
            services.AddHttpClient<IFacebookService, FacebookService>();
            services.AddHttpClient<IWholesaleService, WholesaleService>();
            services.AddHttpClient<IFiberCopService, FiberCopService>();

            services.AddTransient<ISocialService, SocialService>();

            services.AddHostedService<ConfigurationValidationHostedService>();
            services.AddHostedService<CredentialsVerificationHostedService>();
            services.AddHostedService<SchedulerHostedService>();
        }
    }
}
