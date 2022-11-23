using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseConsultaAsignacionLiderEntidadDto
    {
        public string NoRegistrosTotales { get; set; }
        public string Data_IdEPS { get; set; }
        public string Data_NombreEPS { get; set; }

        public string IdAuditorLider { get; set; }
        public string UsuarioAuditor { get; set; }
        public int? IdCobertura { get; set; }
        public int? IdPeriodo { get; set; }
    }
}
