using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class CoberturaDto
    {
        public int IdEnfermedad { get; set; }
        public int IdCobertura { get; set; }
        public string Nombre { get; set; }
        public bool Status { get; set; }
    }
}
