using System.Threading.Tasks;
using Abp.Application.Services;
using BoilerPlateDemo_App.Authorization.Accounts.Dto;

namespace BoilerPlateDemo_App.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
