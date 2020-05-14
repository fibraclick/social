using FibraClickSocial.Configuration;
using FibraClickSocial.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public async Task<DateTimeOffset> GetCurrentVersion()
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Head, this.config.Url);
            HttpResponseMessage resp = await this.client.SendAsync(req);

            if (!resp.IsSuccessStatusCode)
            {
                return default;
            }
            
            string content = await resp.Content.ReadAsStringAsync();

            Match match = Regex.Match(content, "Ultimo aggiornamento copertura: ([0-9]{1,2})/([0-9]{1,2})/([0-9]{4})");

            if (match.Success)
            {
                return new DateTimeOffset(
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[1].Value),
                    0, 0, 0,
                    offset: TimeSpan.Zero
                );
            }
            else
            {
                return default;
            }
        }

        public async Task<DateTimeOffset> GetPreviousVersion(DateTimeOffset fallback)
        {
            if (File.Exists(FILE_PATH))
            {
                string cache = await File.ReadAllTextAsync(FILE_PATH);
                cache = cache.Trim();

                if (cache != "")
                {
                    return DateTimeOffset.FromUnixTimeSeconds(long.Parse(cache));
                }
            }

            await UpdateCurrentVersion(fallback);
            return fallback;
        }

        public Task UpdateCurrentVersion(DateTimeOffset version)
        {
            return File.WriteAllTextAsync(FILE_PATH, version.ToUnixTimeSeconds().ToString());
        }
    }
}
