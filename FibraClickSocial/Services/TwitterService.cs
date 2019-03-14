using FibraClickSocial.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OAuth;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class TwitterService : ITwitterService
    {
        private readonly HttpClient client;
        private readonly TwitterConfiguration config;

        private const string VERIFY_CREDENTIALS_URL =
            "https://api.twitter.com/1.1/account/verify_credentials.json";

        public TwitterService(IOptions<TwitterConfiguration> options,
                              HttpClient client)
        {
            this.config = options.Value;
            this.client = client;
        }

        public async Task<string> VerifyCredentials()
        {
            string auth = GenerateAuthHeader(VERIFY_CREDENTIALS_URL);

            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(VERIFY_CREDENTIALS_URL)
            };

            request.Headers.Add("Authorization", auth);

            HttpResponseMessage response = await this.client.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            JObject parsed = JObject.Parse(content);

            return parsed["screen_name"].ToString();
        }

        private string GenerateAuthHeader(string url)
        {
            OAuthRequest oauth = OAuthRequest.ForProtectedResource(
                method: "GET",
                consumerKey: this.config.ConsumerKey,
                consumerSecret: this.config.ConsumerSecret,
                accessToken: this.config.AccessToken,
                accessTokenSecret: this.config.AccessTokenSecret
            );

            oauth.RequestUrl = url;

            return oauth.GetAuthorizationHeader();
        }
    }
}
