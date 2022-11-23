using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsRegistrosAuditoriaFiltrosDto //: IValidatableObject
    {        
        [Required]
        public string IdAuditor { get; set; } //public int IdAuditor { get; set; }

        //Validamos datos de paginado.
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    //if (IdAuditor != "")
        //    //{
        //    //    yield return new ValidationResult("El IdAuditor debe ser ingresado");
        //    //}            
        //    int Out = 0;
        //    bool NumberValid = int.TryParse(IdAuditor, out Out);
        //    if(NumberValid == false)
        //    {
        //        yield return new ValidationResult("El IdAuditor debe ser un numero");
        //    }

        //    if (Out <= 0)
        //    {
        //        yield return new ValidationResult("El IdAuditor debe ser mayor a 0");
        //    }
        //}
    }
}
