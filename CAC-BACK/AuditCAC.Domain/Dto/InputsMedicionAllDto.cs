using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsMedicionAllDto : IValidatableObject
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        [Key]
        public string Id { get; set; } //int
        public string IdCobertura { get; set; } //int?
        public string IdPeriodo { get; set; } //int?
        public string Descripcion { get; set; }
        public string Activo { get; set; } //bool?
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public int Estado { get; set; }
        public string fechaInicioAuditoria { get; set; }
        public string fechaFinAuditoria { get; set; }
        public string fechaCorteAuditoria { get; set; }
        public string lider { get; set; }
        public string Resolucion { get; set; }
        public string Nombre { get; set; }

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
