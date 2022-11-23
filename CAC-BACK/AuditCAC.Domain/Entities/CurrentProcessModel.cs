using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{

    [Table("Current_Process")]
    public class CurrentProcessModel : BaseEntity
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public int? Progress { get; set; }
        public DateTime InitDate { get; set; }
        public string User { get; set; }
        public string Result { get; set; }
        public bool File { get; set; }
    }

}
