using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsPeriodoDto : IValidatableObject
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        [Key]
        public string idPeriodo { get; set; } //int?
        public string nombre { get; set; }
        public string fechaCorteIni { get; set; } //DateTime?
        public string fechaCorteFin { get; set; } //DateTime?
        public string fechaFinalReporteIni { get; set; } //DateTime?
        public string fechaFinalReporteFin { get; set; } //DateTime?
        public string fechaMaximaCorreccionesIni { get; set; } //DateTime?
        public string fechaMaximaCorreccionesFin { get; set; } //DateTime?
        public string fechaMaximaSoportesIni { get; set; } //DateTime?
        public string fechaMaximaSoportesFin { get; set; } //DateTime?
        public string fechaMaximaConciliacionesIni { get; set; } //DateTime?
        public string fechaMaximaConciliacionesFin { get; set; } //DateTime?
        public string idCobertura { get; set; } //int?
        public string idPeriodoAnt { get; set; } //int?
        public string FechaMinConsultaAuditIni { get; set; } //DateTime?
        public string FechaMinConsultaAuditFin { get; set; } //DateTime?
        public string FechaMaxConsultaAuditIni { get; set; } //DateTime?
        public string FechaMaxConsultaAuditFin { get; set; } //DateTime?


        //Validamos datos de paginado.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageNumber < 0)
            {
                yield return new ValidationResult("El numero de pagina debe ser mayor o igual a 0");
            }

            if (MaxRows <= 0)
            {
                yield return new ValidationResult("El numero de registros maximos 'MaxRows' debe ser mayor a 0");
            }
        }
    }
}
