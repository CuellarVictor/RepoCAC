using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.RegistroAuditoria
{
    public class RegistroAuditoriaInfo
    {
        public string Id { get; set; }
        public int RegistroAuditoriaId { get; set; }

        public string DatoReportado { get; set; }
        public string CampoReferencial { get; set; }
        public string TablaReferencial { get; set; }
        public string Nombre { get; set; }
    }
}
