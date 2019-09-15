using System.Threading.Tasks;

namespace FibraClickSocial.Interfaces
{
    interface IFlashFiberService
    {
        Task<string> GetCurrentVersion();

        Task<string> GetPreviousVersion(string fallback);

        Task UpdateCurrentVersion(string version);
    }
}
