using System;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    interface IWholesaleService
    {
        Task<DateTimeOffset> GetCurrentVersion();

        Task<DateTimeOffset> GetPreviousVersion(DateTimeOffset fallback);

        Task UpdateCurrentVersion(DateTimeOffset version);
    }
}
