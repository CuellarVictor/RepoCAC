using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class UserCredentialsDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }  
        public string Password { get; set; }          
    }
}
