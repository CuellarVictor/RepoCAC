using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    public class MedicionAllModel
    {
        [Key]
        public int Id { get; set; }
        public int? IdCobertura { get; set; }
        public int? IdPeriodo { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Status { get; set; }
    }
}
