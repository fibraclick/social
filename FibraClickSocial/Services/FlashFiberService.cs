using FibraClickSocial.Configuration;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FibraClickSocial.Interfaces;

namespace FibraClickSocial.Services
{
    class FlashFiberService : IFlashFiberService
    {
        private const string FILE_PATH = "flashfiber.txt";

        private readonly HttpClient client;
        private readonly FlashFiberConfiguration config;

        public FlashFiberService(HttpClient client,
                                 IOptions<FlashFiberConfiguration> options)
        {
            this.client = client;
            this.config = options.Value;
        }

        public async Task<string> GetCurrentVersion()
        {
            string resp = await this.client.GetStringAsync(this.config.Url);

            Match match = Regex.Match(resp, "Ultimo aggiornamento copertura: ([0-9]{1,2}/[0-9]{1,2}/[0-9]{4})");

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetPreviousVersion(string fallback)
        {
            if (File.Exists(FILE_PATH))
            {
                string cache = await File.ReadAllTextAsync(FILE_PATH);
                cache = cache.Trim();

                if (cache != "")
                {
                    return cache;
                }
            }

            await UpdateCurrentVersion(fallback);
            return fallback;
        }

        public Task UpdateCurrentVersion(string version)
        {
            return File.WriteAllTextAsync(FILE_PATH, version);
        }
    }
}
