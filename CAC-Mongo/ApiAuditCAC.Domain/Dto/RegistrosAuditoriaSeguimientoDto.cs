using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class RegistrosAuditoriaSeguimientoDto
    {
        public RegistrosAuditoriaSeguimientoDto()
        {

        }
        public int Id { get; set; }
        public int RegistroAuditoriaId { get; set; }
        public int TipoObservacion { get; set; }
        public string TipoObservacionNombre { get; set; }
        public string Observacion { get; set; }
        public int Soporte { get; set; }
        public int EstadoActual { get; set; }
        public string EstadoActualNombre { get; set; }
        public int EstadoNuevo { get; set; }
        public string EstadoNuevoNombre { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public int Status { get; set; }
    }
}
