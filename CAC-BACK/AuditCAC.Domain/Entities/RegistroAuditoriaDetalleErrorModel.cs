using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("RegistroAuditoriaDetalleError")]
    public class RegistroAuditoriaDetalleErrorModel
    {
        [Key]
        public int Id { get; set; }
        public int RegistrosAuditoriaDetalleId { get; set; }
        public int? IdRegla { get; set; }
        public string DescripcionRegla { get; set; }
        public int? IdRestriccion { get; set; }
        public string Reducido { get; set; }
        public int VariableId { get; set; }
        public string ErrorId { get; set; }
        public string Descripcion { get; set; }
        public string Sentencia { get; set; }
        public bool NoCorregible { get; set; }
        public bool Enable { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }    
    }
}
