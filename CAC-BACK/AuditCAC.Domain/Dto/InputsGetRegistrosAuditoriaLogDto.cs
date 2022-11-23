using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsGetRegistrosAuditoriaLogDto
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        public string ParametroBusqueda { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public List<string> IdAuditores { get; set; }
        public int MedicionId { get; set; }

        //Validamos datos de paginado.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageNumber <= 0)
            {
                yield return new ValidationResult("El numero de pagina debe ser mayor a 0");
            }

            if (MaxRows <= 0)
            {
                yield return new ValidationResult("El numero de registros maximos 'MaxRows' debe ser mayor a 0");
            }
        }
    }
}
