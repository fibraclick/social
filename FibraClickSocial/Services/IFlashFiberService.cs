using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    interface IFlashFiberService
    {
        Task<string> GetCurrentVersion();

        Task<string> GetPreviousVersion(string fallback);

        Task UpdateCurrentVersion(string version);
    }
}
