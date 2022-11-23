using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("Variables")]
    public class VariablesModel
    {
        [Key]
        public int Id { get; set; }
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
        public bool Status { get; set; }
    }
}
