using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class UserCredentialsCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public int RolId { get; set; }
        public int oldRolId { get; set; }
        public string Code { get; set; }
        public List<int> Enfermedades { get; set; } 
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
    }
}
