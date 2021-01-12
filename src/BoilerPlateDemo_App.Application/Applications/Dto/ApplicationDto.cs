using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CETAutomation.Applications.Dto
{
    [AutoMapFrom(typeof(CETAutomation.Masters.Application))]
    public class ApplicationDto : EntityDto<int>, IHasCreationTime
    {
        [Required]
        [StringLength(10)]
        public string ApplicationName { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
