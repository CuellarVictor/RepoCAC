using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsRegistrosAuditoriaDetalleUpdate_Actualizar_DC_NC_ND_Motivo_Dto
    {
        public int RegistroauditoriaId { get; set; }
        public string UserId { get; set; }
        public int RegistrosAuditoriaDetalle { get; set; }
        public string MotivoVariable { get; set; } 
        public int Dato_DC_NC_ND { get; set; }
    }
}
