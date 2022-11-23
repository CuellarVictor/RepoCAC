using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities //AuditCAC.Dal.Entities
{
    public class PeriodoModel
    {
        //[Key]
        //public int? idPeriodo { get; set; }
        //public string nombre { get; set; }
        //public DateTime? fechaCorteIni { get; set; }
        //public DateTime? fechaCorteFin { get; set; }
        //public DateTime? fechaFinalReporteIni { get; set; }
        //public DateTime? fechaFinalReporteFin { get; set; }
        //public DateTime? fechaMaximaCorreccionesIni { get; set; }
        //public DateTime? fechaMaximaCorreccionesFin { get; set; }
        //public DateTime? fechaMaximaSoportesIni { get; set; }
        //public DateTime? fechaMaximaSoportesFin { get; set; }
        //public DateTime? fechaMaximaConciliacionesIni { get; set; }
        //public DateTime? fechaMaximaConciliacionesFin { get; set; }
        //public int? idCobertura { get; set; }
        //public int? idPeriodoAnt { get; set; }
        //public DateTime? FechaMinConsultaAuditIni { get; set; }
        //public DateTime? FechaMinConsultaAuditFin { get; set; }
        //public DateTime? FechaMaxConsultaAuditIni { get; set; }
        //public DateTime? FechaMaxConsultaAuditFin { get; set; }

        //
        [Key]
        public int? idPeriodo { get; set; }
        public string nombre { get; set; }
        public DateTime? fechaCorte { get; set; }
        public DateTime? fechaFinalReporte { get; set; }
        public DateTime? fechaMaximaCorrecciones { get; set; }
        public DateTime? fechaMaximaSoportes { get; set; }
        public DateTime? fechaMaximaConciliaciones { get; set; }
        public int? idCobertura { get; set; }
        public int? idPeriodoAnt { get; set; }
        public DateTime? FechaMinConsultaAudit { get; set; }
        public DateTime? FechaMaxConsultaAudit { get; set; }
    }
}
