using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.Actas
{
    public class ParametroMedicionDetalleDto
    {
        public string Medicion { get; set; }
        public decimal Conforme { get; set; }
        public decimal NoConforme { get; set; }
        public decimal NoDisponible { get; set; }
        public decimal CasosAuditados { get; set; }
    }
}
