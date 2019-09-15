using System.Threading.Tasks;

namespace FibraClickSocial.Interfaces
{
    interface IVerifyCredentials
    {
        Task<string> VerifyCredentials();
    }
}
