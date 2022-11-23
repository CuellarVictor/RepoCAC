using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsCreateVariablesDto
    {
        //[Key]      
        //public int Id { get; set; }
        public bool Activa { get; set; }
        public int? Orden { get; set; }
        public int? idVariable { get; set; }
        public int? idCobertura { get; set; }
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string descripcion { get; set; }
        public string idTipoVariable { get; set; }
        public int? longitud { get; set; }
        public int? decimales { get; set; }
        public string formato { get; set; }        
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string MotivoVariable { get; set; }
        public bool Bot { get; set; }
        public int TipoVariableItem { get; set; }
        public int EstructuraVariable { get; set; }

        //
        public int? MedicionId { get; set; } //Foranea
        public bool EsGlosa { get; set; }
        public bool EsVisible { get; set; }
        public bool EsCalificable { get; set; }
        public bool Activo { get; set; }
        public bool EnableDC { get; set; }
        public bool EnableNC { get; set; }
        public bool EnableND { get; set; }        
        public bool CalificacionXDefecto { get; set; }
        public int? SubGrupoId { get; set; }
        public bool Encuesta { get; set; }
        public int? VxM_Orden { get; set; }
        //
    }
}
