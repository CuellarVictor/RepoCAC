using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseConsultarEstadosEntidadDto
    {
        public int IdRadicado { get; set; }
        public string IdEPS { get; set; }
        public string Observacion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int EstadoActual { get; set; }
        public int EstadoNuevo { get; set; }
    }
}
