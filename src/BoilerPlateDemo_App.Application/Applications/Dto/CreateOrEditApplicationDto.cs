using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CETAutomation.Applications.Dto
{
  public  class CreateOrEditApplicationDto : EntityDto<int?>
    {
        [Required]
        [StringLength(10)]
        public string ApplicationName { get; set; }
    }
}
