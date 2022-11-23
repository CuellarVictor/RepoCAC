using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseGetUsuariosByRoleIdDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }        
    }
}
