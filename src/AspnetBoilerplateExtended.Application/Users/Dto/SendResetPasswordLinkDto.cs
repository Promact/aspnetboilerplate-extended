using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspnetBoilerplateExtended.Users.Dto
{
    public class SendResetPasswordLinkDto
    {
        [Required]
        public string Email { get; set; }
    }
}
