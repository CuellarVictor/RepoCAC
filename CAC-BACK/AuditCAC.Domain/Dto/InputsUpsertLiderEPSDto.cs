using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsUpsertLiderEPSDto
    {
        public string IdEPS { get; set; }
        public string IdAuditorLider { get; set; }
        public int IdCobertura { get; set; }
        public int IdPeriodo { get; set; }
        public string Usuario { get; set; }
    }
}
