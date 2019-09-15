using System.Threading.Tasks;

namespace FibraClickSocial.Interfaces
{
    interface ISocialService
    {
        Task Publish(string telegram, string twitter, string facebook);
    }
}
