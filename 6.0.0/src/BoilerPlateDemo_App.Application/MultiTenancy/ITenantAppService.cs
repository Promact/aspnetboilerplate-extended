using Abp.Application.Services;
using BoilerPlateDemo_App.MultiTenancy.Dto;

namespace BoilerPlateDemo_App.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

