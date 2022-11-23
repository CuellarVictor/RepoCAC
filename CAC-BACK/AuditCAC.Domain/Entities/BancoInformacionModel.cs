using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("BancoInformacion")]
    public class BancoInformacionModel
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? Tipo { get; set; }
        public string Codigo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Status { get; set; }
        public int IdCobertura { get; set; }
    }
}
