using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsProcessLogFiltradoDto
    {
        //Paginado        
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        //
        public string ProcessLogId { get; set; }
        public string ProcessId { get; set; }
        [Required]
        public string User { get; set; }
        public string DateIni { get; set; }
        public string DateFin { get; set; }
        public string Result { get; set; }
        public string Observation { get; set; }
        public string PalabraClave { get; set; }


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
