using System.Threading.Tasks;

namespace FibraClickSocial.Interfaces
{
    interface ISendMessage
    {
        Task SendMessageAsync(string text);
    }
}
