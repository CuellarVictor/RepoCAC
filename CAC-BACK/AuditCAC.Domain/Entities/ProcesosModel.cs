using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("Process")]
    public class ProcessModel
    {
        [Key]
        public int ProcessId { get; set; }
        public int? Status { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public int? LifeTime { get; set; }
    }
}
