using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    interface ITelegramService : IVerifyCredentials
    {
        Task SendMessage(string text);
    }
}
