using FibraClickSocial.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FibraClickSocial.Services
{
    class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient client;
        private readonly string CHANNEL_NAME;

        public TelegramService(IOptions<TelegramConfiguration> options,
                               HttpClient http)
        {
            CHANNEL_NAME = "@" + options.Value.ChannelUsername;
            this.client = new TelegramBotClient(options.Value.BotToken, http);
        }

        public async Task<string> VerifyCredentials()
        {
            User me = await this.client.GetMeAsync();

            return me.Username;
        }

        public Task SendMessage(string text)
        {
            return this.client.SendTextMessageAsync(CHANNEL_NAME, text, ParseMode.Markdown, disableWebPagePreview: true);
        }
    }
}
