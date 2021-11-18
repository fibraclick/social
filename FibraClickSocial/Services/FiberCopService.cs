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
    class FiberCopService : IFiberCopService
    {
        private const string FILE_PATH = "fibercop.txt";

        private readonly HttpClient client;
        private readonly FiberCopConfiguration config;

        public FiberCopService(HttpClient client,
            IOptions<FiberCopConfiguration> options)
        {
            this.client = client;
            this.config = options.Value;
        }

        public async Task<string> GetCurrentCount()
        {
            HttpResponseMessage resp = await this.client.GetAsync(this.config.Url);

            if (!resp.IsSuccessStatusCode)
            {
                return default;
            }

            string content = await resp.Content.ReadAsStringAsync();

            MatchCollection matches = Regex.Matches(content, "{lat: \\d+\\.\\d+,lng: \\d+\\.\\d+ }");

            if (matches.Count == 0)
            {
                return default;
            }
            else
            {
                return matches.Count.ToString();
            }
        }

        public async Task<string> GetPreviousCount()
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

            return default;
        }

        public Task UpdateCurrentVersion(string version)
        {
            return File.WriteAllTextAsync(FILE_PATH, version);
        }
    }
}
