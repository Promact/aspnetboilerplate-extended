using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CETAutomation.Masters
{
   public class Application:FullAuditedEntity
    {
        /// <summary>
        /// Name of application
        /// </summary>
        [StringLength(10)]
        [Required]
        
        public string ApplicationName { get; set; }
    }
}
