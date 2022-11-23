using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("VariableXMedicion")]
    public class VariableXMedicionModel
    {
        [Key]
        public int? VariableId { get; set; } //Foranea
        //[Key]
        public int? MedicionId { get; set; } //Foranea
        public bool EsGlosa { get; set; }
        public bool EsVisible { get; set; }
        public bool EsCalificable { get; set; }
        public bool Activo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool EnableDC { get; set; }
        public bool EnableNC { get; set; }
        public bool EnableND { get; set; }
        public int? CalificacionXDefecto { get; set; }
        public int? SubGrupoId { get; set; }
        public bool Encuesta { get; set; }
        public int? Orden { get; set; }
        public bool Status { get; set; }
    }
}
