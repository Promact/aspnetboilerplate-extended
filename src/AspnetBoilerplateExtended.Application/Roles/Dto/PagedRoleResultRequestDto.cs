using Abp.Application.Services.Dto;

namespace AspnetBoilerplateExtended.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

