using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    [Keyless]
    public class ResponseSPGetDataMigPeriodo
    {
        public Int32 idPeriodo { get; set; }
        public string nombre { get; set; }
        public DateTime fechaCorte { get; set; }
        public DateTime fechaFinalReporte { get; set; }
        public DateTime fechaMaximaCorrecciones { get; set; }
        public DateTime fechaMaximaSoportes { get; set; }
        public DateTime fechaMaximaConciliaciones { get; set; }
        public Int32 idCobertura { get; set; }
        public Nullable<Int32> idPeriodoAnt { get; set; }
        public Nullable<DateTime> FechaMinConsultaAudit { get; set; }
        public Nullable<DateTime> FechaMaxConsultaAudit { get; set; }

    }
}
