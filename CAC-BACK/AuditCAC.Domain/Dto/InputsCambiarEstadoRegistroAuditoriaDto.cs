using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsCambiarEstadoRegistroAuditoriaDto
    {
        public int RegistroAuditoriaId { get; set; }
        public string Proceso { get; set; }
        public string Observacion { get; set; }
        public int EstadoAnterioId { get; set; }
        public int EstadoActual { get; set; }
        public string AsignadoA { get; set; }
        public string AsingadoPor { get; set; }
        public string CreatedBy { get; set; }
        //public int IdEstadoNuevo { get; set; }
    }
}
