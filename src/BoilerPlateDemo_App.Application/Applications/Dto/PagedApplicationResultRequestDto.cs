using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CETAutomation.Applications.Dto
{
    public class PagedApplicationResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
