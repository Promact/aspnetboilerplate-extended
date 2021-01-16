using System.Threading.Tasks;
using Abp.Application.Services;
using BoilerPlateDemo_App.Sessions.Dto;

namespace BoilerPlateDemo_App.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
