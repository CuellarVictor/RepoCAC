using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsActualizarDC_NC_ND_MotivoDto //InputsActualizarDC_NC_ND_MotivoDto
    {
        public InputsActualizarDC_NC_ND_MotivoDto() //InputsActualizarDC_NC_ND_MotivoDto
        {

        }

        //public int RegistroAuditoriaId  { get; set; }
        //public int IdEstadoNuevo { get; set; }
        public int RegistroAuditoriaId { get; set; }
        public string Proceso { get; set; }
        public string Observacion { get; set; }
        public int EstadoAnterioId { get; set; }
        public int EstadoActual { get; set; }
        public string AsignadoA { get; set; }
        public string AsingadoPor { get; set; }
        public string CreatedBy { get; set; }
        public List<ListResponseRegistroAuditoriaDetalle> ListadoCalificaciones { get; set; }
        
        public class ListResponseRegistroAuditoriaDetalle 
        {
            public string name { get; set; }
            public string error { get; set; }
            public List<ListVariable> variables { get; set; }

            public class ListVariable
            {
                public string reducido { get; set; }
                public string estado { get; set; }
                public string detalle { get; set; }
                public string hallazgo { get; set; }
                public List<string> error { get; set; }
                public string seleccion { get; set; }
                public string dato_reportado { get; set; }
                public string motivo { get; set; }
                public List<string> listaMotivos { get; set; }
                public int registroAuditoriaDetalleId { get; set; }
                public int? Dato_DC_NC_ND { get; set; }
                public bool? Visible { get; set; }
                public bool? Bot { get; set; }
            }
        }
    }
}
