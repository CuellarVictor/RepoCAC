using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsRadicadoDto : IValidatableObject
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        [Key]
        public string idRadicado { get; set; }
        public string idArchivoRecibido { get; set; } //int?
        public string fechaRadicadoIni { get; set; }
        public string fechaRadicadoFin { get; set; }
        public string reemplazado { get; set; } //public bool? reemplazado { get; set; }
        public string fechaReemplazadoIni { get; set; }
        public string fechaReemplazadoFin { get; set; }
        public string observaciones { get; set; }
        public string idRadicadoReemplazado { get; set; }

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
