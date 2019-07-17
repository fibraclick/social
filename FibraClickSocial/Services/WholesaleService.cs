using FibraClickSocial.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class WholesaleService : IWholesaleService
    {
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

            return resp.Content.Headers.LastModified.GetValueOrDefault();
        }
    }
}
