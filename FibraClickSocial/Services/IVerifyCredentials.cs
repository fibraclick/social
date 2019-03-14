using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    interface IVerifyCredentials
    {
        Task<string> VerifyCredentials();
    }
}
