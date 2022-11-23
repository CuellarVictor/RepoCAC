using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditCAC.Domain.Entities
{
    [Table("CatalogoItemCobertura")]
    public class ItemCoberturaModel
    {
        [Key]
        public int Id { get; set; }
        public int? CatalogoCoberturaId { get; set; }
        public string ItemId { get; set; }
        public string ItemDescripcion { get; set; }
        public bool? ItemActivo { get; set; }
        public int? ItemOrden { get; set; }
        public bool? ItemGlosa { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
