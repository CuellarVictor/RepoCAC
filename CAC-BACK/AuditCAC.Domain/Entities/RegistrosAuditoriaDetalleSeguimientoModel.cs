using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("RegistrosAuditoriaDetalleSeguimiento")]
    public class RegistrosAuditoriaDetalleSeguimientoModel: BaseEntity //: IValidatableObject
    {
        //[Key]
        //public int Id { get; set; }
        public int? RegistroAuditoriaId { get; set; } //RegistroAuditoriaDetalleId
        public int? TipoObservacion { get; set; }
        public string Observacion { get; set; }
        public int? Soporte { get; set; }
        [Required]
        public int? EstadoActual { get; set; }        
        [Required]
        public int? EstadoNuevo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Status { get; set; }

        ////Validamos datos de paginado.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Observacion.Length > 300)
            {
                yield return new ValidationResult("El numero maximo de caracteres admitidos es 300");
            }
        }
    }
}
