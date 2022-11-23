using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsGetUsuariosByRoleCoberturaIdDto
    {
        public int RoleId { get; set; }
        public int CoberturaId { get; set; }
        public string UserId { get; set; }
    }
}
