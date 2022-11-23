using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class HallazgosDto
    {
        public HallazgosDto()
        {

        }

        public int Id { get; set; }
        public int RegistrosAuditoriaDetalleId { get; set; }
        public string Observacion { get; set; }
        public int Estado { get; set; }
        public string EstadoNombre { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Status { get; set; }
        public int Dato_DC_NC_ND { get; set; }
        public string Dato_DC_NC_ND_Nombre { get; set; }
    }
}
