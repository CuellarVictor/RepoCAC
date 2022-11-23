using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuditCAC.MainCore.Module.Interface
{
    public class InputsProcesosDto
    {
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        [Key]
        public int ProccesId { get; set; }
        public string? Status { get; set; }
        public string? Name { get; set; }
        public string? Class { get; set; }
        public string? Method { get; set; }
        public string? LifeTime { get; set; }

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