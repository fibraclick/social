using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    interface ISendMessage
    {
        Task SendMessageAsync(string text);
    }
}
