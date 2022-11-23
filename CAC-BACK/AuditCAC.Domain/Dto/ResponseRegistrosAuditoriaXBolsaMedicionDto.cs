using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistrosAuditoriaXBolsaMedicionDto
    {
        public string QueryNoRegistrosTotales { get; set; }
        public string IdAuditor { get; set; }
        public string NombreAuditor { get; set; }
        public int? IdRadicado { get; set; }
        public int? Estado { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public string CodigoUsuario { get; set; }
        public int IdMedicion { get; set; }
        public string NombreMedicion { get; set; }
        public string Data_IdEPS { get; set; }
        public string Data_NombreEPS { get; set; }
        public string IPS { get; set; }
        public int IdEnfermedadMadre { get; set; }
        public string NombreEnfermedadMadre { get; set; }
    }
}
