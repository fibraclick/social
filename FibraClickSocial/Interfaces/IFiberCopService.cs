using System;
using System.Threading.Tasks;

namespace FibraClickSocial.Interfaces
{
    interface IFiberCopService
    {
        Task<string> GetCurrentCount();

        Task<string> GetPreviousCount();

        Task UpdateCurrentVersion(string version);
    }
}
