using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("VariablesXItems")]
    public class VariablesXItemsModel
    {
        [Key]
        public int Id { get; set; }
        public int? VariablesId { get; set; }
        public int? ItemId { get; set; }
        //public bool EsGlosa { get; set; }
        //public bool EsVisible { get; set; }
        //public bool EsCalificable { get; set; }
        //public bool Activo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Status { get; set; }
    }
}
