using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputReasignacionesBolsaEquitativaDto : IValidatableObject
    {
        //[Required]
        //public List<int> MedionId { get; set; } //public int MedionId { get; set; }    
        [Required]
        public List<string> AuditoresId { get; set; }
        [Required]
        public List<int> IdRadicado { get; set; } //RegistrosAuditoriaId
        public string IdUsuario { get; set; }

        //Validamos formulario
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AuditoresId.Count > IdRadicado.Count)
            {
                yield return new ValidationResult("El numero Numero de registros seleccionados debe ser superior al de Auditores.");
            }

            if (AuditoresId.Count == 0)
            {
                yield return new ValidationResult("El numero de Auditores debe ser mayor a 0");
            }
        }
    }
}
