using FibraClickSocial.Configuration;
using FibraClickSocial.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PlainHttp;

namespace FibraClickSocial.Services
{
    class FiberCopService : IFiberCopService
    {
        private const string FILE_PATH = "fibercop.txt";

        private readonly HttpClient client;
        private readonly FiberCopConfiguration config;

        private DateTime lastChangedAt;

        public FiberCopService(HttpClient client,
            IOptions<FiberCopConfiguration> options)
        {
            this.client = client;
            this.config = options.Value;
        }

        public async Task<string> GetCurrentCount()
        {
            IHttpRequest req = new HttpRequest(this.config.Url)
            {
                Headers = new Dictionary<string, string>()
                {
                    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:101.0) Gecko/20100101 Firefox/101.0" }
                }
            };

            IHttpResponse resp = await req.SendAsync();
            resp.Message.EnsureSuccessStatusCode();

            MatchCollection matches = Regex.Matches(resp.Body, "lat: \\d+\\.\\d+");

            if (matches.Count == 0)
            {
                throw new Exception("No matching line found");
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

        public Task UpdateCurrentCount(string count)
        {
            return File.WriteAllTextAsync(FILE_PATH, count);
        }

        public void SetNotified()
        {
            this.lastChangedAt = DateTime.Now;
        }

        public bool ShouldNotify()
        {
            return DateTime.Now - this.lastChangedAt > TimeSpan.FromHours(6);
        }
    }
}
