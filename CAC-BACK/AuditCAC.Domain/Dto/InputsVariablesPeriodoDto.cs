using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsVariablesPeriodoDto: IValidatableObject
    {
        //Paginado

        public int PageNumber { get; set; } = 0;

        public int MaxRows { get; set; } = 1000;
        [Key]
        public string idPeriodo { get; set; } //int?
        public string idVariable { get; set; } //int?
        public string orden { get; set; } //int?
        public string variable_num_segun_resoucion { get; set; }

        //Validamos datos de paginado.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(PageNumber < 0)
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
