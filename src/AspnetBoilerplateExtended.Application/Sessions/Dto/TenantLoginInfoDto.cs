using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AspnetBoilerplateExtended.MultiTenancy;

namespace AspnetBoilerplateExtended.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
