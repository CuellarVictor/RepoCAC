using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseCambiarEstadoEntidadDto
    {
        public string usuarioId { get; set; }
        public string radicadoId { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
    }
}
