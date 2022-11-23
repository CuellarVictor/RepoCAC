using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsVariableDto : IValidatableObject
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        [Key]
        public string idVariable { get; set; } //int?
        public string idCobertura { get; set; } //int?
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string descripcion { get; set; }
        public string idTipoVariable { get; set; }
        public string longitud { get; set; } //int?
        public string decimales { get; set; } //int?
        public string formato { get; set; }
        public string idErrorTipo { get; set; }
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }
        public string idErrorReferencial { get; set; }
        public string idTipoVariableAlterno { get; set; }        
        public string formatoAlterno { get; set; }        
        public string permiteVacio { get; set; } //public bool? permiteVacio { get; set; }
        public string idErrorPermiteVacio { get; set; } //public bool? identificadorRegistro { get; set; }
        public string identificadorRegistro { get; set; } //public bool? clavePrimaria { get; set; }
        public string clavePrimaria { get; set; }
        public string idTipoAnalisisEpidemiologico { get; set; } //byte?
        public string sistema { get; set; } //public bool? sistema { get; set; }
        public string exportable { get; set; } //public bool? exportable { get; set; }
        public string enmascarado { get; set; } //public bool? enmascarado { get; set; }


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
