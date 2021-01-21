using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CETAutomation.Masters
{
  public  class Project : FullAuditedEntity
    {
        /// <summary>
        /// Defines name of project
        /// </summary>

        [StringLength(200)]
       
        public string Name { get; set; }

        /// <summary>
        /// Defines description of project
        /// </summary>
        [StringLength(1000)]
        
        public string Description { get; set; }
    }
}
