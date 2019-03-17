using FibraClickSocial.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class FacebookService : IFacebookService
    {
        private readonly FacebookConfiguration config;
        private readonly HttpClient client;

        private readonly string[] REQUIRED_SCOPES = new[] { "manage_pages", "publish_pages" };

        private const string VERIFY_SCOPES_URL =
            "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";

        private const string ME_URL =
            "https://graph.facebook.com/v3.2/me?fields=id,name&access_token={0}";

        private const string PUBLISH_URL =
            "https://graph.facebook.com/v3.2/me/feed";

        public FacebookService(IOptions<FacebookConfiguration> options,
                               HttpClient client)
        {
            this.config = options.Value;
            this.client = client;
        }

        public async Task<string> VerifyCredentials()
        {
            string url = string.Format(
                VERIFY_SCOPES_URL,
                this.config.AccessToken,
                this.config.AppId,
                this.config.AppSecret);

            HttpResponseMessage response = await this.client.GetAsync(url);

            await ManageError(response);

            string content = await response.Content.ReadAsStringAsync();

            JObject parsed = JObject.Parse(content);

            bool isValid = parsed.SelectToken("data.is_valid").ToObject<bool>();

            if (!isValid)
            {
                throw new FacebookException($"Token is not valid (may be expired)");
            }

            string type = parsed.SelectToken("data.type").ToString();

            if (type != "PAGE")
            {
                throw new FacebookException($"Wrong token type: {type}");
            }

            JArray scopes = (JArray)parsed.SelectToken("data.scopes");

            foreach (string scope in REQUIRED_SCOPES)
            {
                if (scopes.FirstOrDefault(x => x.ToString() == scope) == null)
                {
                    throw new FacebookException($"Missing scope '{scope}'");
                }
            }

            response = await this.client.GetAsync(string.Format(ME_URL, this.config.AccessToken));
            response.EnsureSuccessStatusCode();

            content = await response.Content.ReadAsStringAsync();

            JObject me = JObject.Parse(content);

            return me["name"].ToString();
        }

        public async Task SendMessageAsync(string text)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "message", text },
                { "access_token", this.config.AccessToken }
            };

            HttpResponseMessage response =
                await this.client.PostAsync(PUBLISH_URL, new FormUrlEncodedContent(data));

            await ManageError(response);
        }

        private async Task ManageError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();

                throw new Exception($"Facebook response error: {body.Replace('\n', '|')}");
            }
        }
    }
}
