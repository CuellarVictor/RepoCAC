using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("Item")]
    public class ItemModel : BaseEntity //: IValidatableObject
    {
        //public int Id { get; set; }
        public int CatalogId { get; set; }
        public string ItemName { get; set; }
        public string Concept { get; set; }
        public bool Enable { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime CreateDate { get; set; } 
        public bool Status { get; set; }
        //public string UsuarioId { get; set; }
    }
}
