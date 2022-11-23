using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseUsuariosBolsaMedicionDto
    {
        public string QueryNoRegistrosTotales { get; set; }
        public string AuditorUsuario { get; set; }
        public string AuditorNombres { get; set; }
        public string AuditorApellidos { get; set; }
        public string AuditorCodigo { get; set; }
        public int RegistrosAsignados { get; set; }
        public int RegistrosAuditados { get; set; }
        public string AuditorEstado { get; set; }
    }
}
