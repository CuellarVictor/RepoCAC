using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputRegistraRespuestaHallazgosDto
    {
        public int RegistroAuditoriaDetalleId { get; set; }
        public int Estado { get; set; }
        public string Observacion { get; set; }
        public string Usuario { get; set; }

    }
}
