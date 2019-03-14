using FibraClickSocial.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FibraClickSocial.Services
{
    class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient client;

        public TelegramService(IOptions<TelegramConfiguration> options,
                               HttpClient http)
        {
            this.client = new TelegramBotClient(options.Value.BotToken, http);
        }

        public async Task<string> VerifyCredentials()
        {
            User me = await this.client.GetMeAsync();

            return me.Username;
        }
    }
}
