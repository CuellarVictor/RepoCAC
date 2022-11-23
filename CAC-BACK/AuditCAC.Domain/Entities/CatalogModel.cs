using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("Catalog")]
    public class CatalogModel : BaseEntity //: IValidatableObject
    {
        //public int Id { get; set; }
        public string CatalogName { get; set; }
        public bool Enable { get; set; }
        public DateTime LastModify { get; set; } 
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
