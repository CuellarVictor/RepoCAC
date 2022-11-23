using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.Actas
{
    public class ParametroMedicionInconsistenciaDto
    {
        public string Medicion { get; set; }
        public decimal CasosAuditados { get; set; }
        public decimal InconsistenciasPorDiagnostico { get; set; }
        public decimal InconsistenciasPorSoportes { get; set; }
        public string PorcentajeGlosa { get; set; }
    }
}
