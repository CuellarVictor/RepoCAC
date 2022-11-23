using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseConsultarHallazgosDto
    {
        public int Dato_DC_NC_NDId { get; set; }
        public string Dato_DC_NC_NDNombre { get; set; }
        public string HallazgoObservacion { get; set; }
        public int HallazgoEstadoId { get; set; }
        public string HallazgoEstadoNombre { get; set; }
        public int RegistrosAuditoriaDetalleId { get; set; }
        public int IdRadicado { get; set; }
        public int RAEstado { get; set; }
        public string ERADCodigo { get; set; }
        public string ERADNombre { get; set; }
    }
}
