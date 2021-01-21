using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using CETAutomation.Export;
using CETAutomation.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CETAutomation.Applications.Dto
{
    [AutoMapFrom(typeof(CETAutomation.Masters.Application))]
    public class ApplicationDto 
    {


        //Export(IsAllowExport)=false attribute will ignore this column while export

        [Export(IsAllowExport = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]

        public string ApplicationName { get; set; }

        [Export(IsAllowExport = false)]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Project is required")]
        public string ProjectName { get; set; }

        [Export(IsAllowExport = false)]
        public DateTime CreationTime { get; set; }


        [Export(IsAllowExport = false)]
        public Project project { get; set; }


        [DisplayName("CreationTime")]
        public string Time { get; set; }




    }
}
