using FibraClickSocial.Configuration;
using LinqToTwitter;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    class TwitterService : ITwitterService
    {
        private readonly TwitterContext client;

        private const string VERIFY_CREDENTIALS_URL =
            "https://api.twitter.com/1.1/account/verify_credentials.json";

        private const string PUBLISH_URL =
            "https://api.twitter.com/1.1/statuses/update.json";

        public TwitterService(IOptions<TwitterConfiguration> options,
                              HttpClient client)
        {
            this.client = new TwitterContext(new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = options.Value.ConsumerKey,
                    ConsumerSecret = options.Value.ConsumerSecret,
                    AccessToken = options.Value.AccessToken,
                    AccessTokenSecret = options.Value.AccessTokenSecret
                }
            });
        }

        public async Task<string> VerifyCredentials()
        {
            Account account = await this.client.Account
                .Where(x => x.Type == AccountType.VerifyCredentials)
                .FirstAsync();

            return account.User.ScreenNameResponse;
        }

        public Task SendMessageAsync(string text)
        {
            return this.client.TweetAsync(text);
        }
    }
}
