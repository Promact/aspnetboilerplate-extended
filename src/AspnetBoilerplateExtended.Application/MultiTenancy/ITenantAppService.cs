using Abp.Application.Services;
using AspnetBoilerplateExtended.MultiTenancy.Dto;

namespace AspnetBoilerplateExtended.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

