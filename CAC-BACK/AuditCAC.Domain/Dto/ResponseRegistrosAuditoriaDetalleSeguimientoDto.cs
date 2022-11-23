using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistrosAuditoriaDetalleSeguimientoDto
    {
        public int Id { get; set; }
        public int? RegistroAuditoriaId { get; set; }
        public int? TipoObservacion { get; set; }
        public string NombreTipoObservacion { get; set; }
        public string Observacion { get; set; }
        public int? Soporte { get; set; }        
        public int? EstadoActual { get; set; }
        public string EstadoActualNombre { get; set; }
        public int? EstadoNuevo { get; set; }
        public string EstadoNuevoNombre { get; set; }
        public string IdRol { get; set; } //public int? IdRol { get; set; }
        public string NombreRol { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string UserName { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Codigo { get; set; }

    }
}
