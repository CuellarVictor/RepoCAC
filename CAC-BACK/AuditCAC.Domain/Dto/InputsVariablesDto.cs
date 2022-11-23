using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    [Table("Variables")]
    public class InputsVariablesDto
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        //[Key]        
        public string Id { get; set; } //List<string>
        public string Variable { get; set; } 
        public string Activa { get; set; } // Bool
        public string Orden { get; set; } //int?
        public string idVariable { get; set; } //int?
        public string idCobertura { get; set; } //int?
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string descripcion { get; set; }
        public string idTipoVariable { get; set; }
        public string longitud { get; set; } //int?
        public string decimales { get; set; } //int?
        public string formato { get; set; }
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string MotivoVariable { get; set; }
        public string Bot { get; set; } //bool
        public List<string> TipoVariableItem { get; set; }
        public string EstructuraVariable { get; set; }

        //

        public string MedicionId { get; set; }
        public string EsGlosa { get; set; }
        public string EsVisible { get; set; }
        public string EsCalificable { get; set; }
        public string Activo { get; set; }
        public string EnableDC { get; set; }
        public string EnableNC { get; set; }
        public string EnableND { get; set; }
        public string CalificacionXDefecto { get; set; }
        public List<string> SubGrupoId { get; set; } //public string SubGrupoId { get; set; }               
        public string Encuesta { get; set; }
        public string VxM_Orden { get; set; }

        public string Alerta { get; set; }
        public string AlertaDescripcion { get; set; }
        public string calificacionIPSItem { get; set; }
        public string IdRegla { get; set; }
        public string Concepto { get; set; }
        //public string TipoVariableItemNombre { get; set; }

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
