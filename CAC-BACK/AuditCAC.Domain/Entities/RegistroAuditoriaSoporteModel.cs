using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("RegistroAuditoriaSoporte")]
    public class RegistroAuditoriaSoporteModel
    {
        [key]
        public int Id { get; set; } //public long Id { get; set; }, Int64. Validar tipo de dato Bigint.
        public int? IdRegistrosAuditoria { get; set; } // public int? IdRegistrosAuditoria { get; set; }
        public int? IdSoporte { get; set; } // public int? IdSoporte { get; set; }
        public int? IdEntidad { get; set; } //public int? IdEntidad { get; set; }
        public string NombreSoporte { get; set; }
        public string UrlSoporte { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Status { get; set; }
    }
}
