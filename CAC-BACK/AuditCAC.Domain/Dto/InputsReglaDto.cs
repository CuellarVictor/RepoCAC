using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsReglaDto : IValidatableObject
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        [Key]
        public string idRegla { get; set; } //int?
        public string idCobertura { get; set; } //int?
        public string nombre { get; set; }
        public string idTipoRegla { get; set; }
        public string idTiempoAplicacion { get; set; } //byte?
        public string habilitado { get; set; } //public bool? habilitado { get; set; }
        public string idError { get; set; }
        public string idVariable { get; set; } //int?
        public string idTipoEnvioLimbo { get; set; } //byte?

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
