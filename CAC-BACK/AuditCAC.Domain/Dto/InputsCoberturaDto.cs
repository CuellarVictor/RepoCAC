using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsCoberturaDto : IValidatableObject
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        [Key]
        public string idCobertura { get; set; } //int?
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string legislacion { get; set; }
        public string definicion { get; set; }
        public string tiempoEstimado { get; set; } //int?
        public string ExcluirNovedades { get; set; } //public bool? ExcluirNovedades { get; set; }
        public string Novedades { get; set; }
        public string idResolutionSiame { get; set; } //int?
        public string NovedadesCompartidosUnicos { get; set; }
        public string CantidadVariables { get; set; } //int?
        public string idCoberturaPadre { get; set; } //int?
        public string idCoberturaAdicionales { get; set; } //int?

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
