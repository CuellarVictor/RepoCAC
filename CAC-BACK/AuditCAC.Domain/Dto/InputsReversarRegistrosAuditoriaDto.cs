using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsReversarRegistrosAuditoriaDto
    {
        public int IdRegistrosAuditoria { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public string IdUsuario { get; set; }
    }
}
