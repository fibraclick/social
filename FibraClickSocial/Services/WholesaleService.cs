using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FibraClickSocial.Configuration;
using FibraClickSocial.Interfaces;
using Microsoft.Extensions.Options;

namespace FibraClickSocial.Services
{
    class WholesaleService : IWholesaleService
    {
        private const string FILE_PATH = "version.txt";

        private readonly HttpClient client;
        private readonly WholesaleConfiguration config;

        public WholesaleService(HttpClient client,
            IOptions<WholesaleConfiguration> options)
        {
            this.client = client;
            this.config = options.Value;
        }

        public async Task<DateTimeOffset> GetCurrentVersion()
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Head, this.config.Url);

            HttpResponseMessage resp = await this.client.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            return resp.Content.Headers.LastModified.GetValueOrDefault();
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
