using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CETAutomation.Applications.Dto
{
   public class GetAllApplicationInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }


    }
}
