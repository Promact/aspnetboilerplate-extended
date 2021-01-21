using System.Threading.Tasks;
using Abp.Application.Services;
using AspnetBoilerplateExtended.Sessions.Dto;

namespace AspnetBoilerplateExtended.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
