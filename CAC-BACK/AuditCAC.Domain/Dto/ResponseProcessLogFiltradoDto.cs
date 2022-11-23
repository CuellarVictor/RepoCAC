using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseProcessLogFiltradoDto
    {
        public int? NoRegistrosTotales { get; set; }
        public int? ProcessLogId { get; set; }
        public int? IdActividad { get; set; }
        public string NombreActividad { get; set; }
        public string DescripcionActividad { get; set; }
        public string IdUsuario { get; set; }

        public string Usuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime? Date { get; set; }
        public string Result { get; set; }
        public string Observation { get; set; }
    }
}
