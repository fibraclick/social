using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    interface ISocialService
    {
        Task Publish(string telegram, string twitter, string facebook);
    }
}
