using System;
using System.Threading.Tasks;

namespace FibraClickSocial.Services
{
    interface IWholesaleService
    {
        Task<DateTimeOffset> GetCurrentVersion();
    }
}
