using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputDestalleAsignacion
    {
        [Required]
        public List<string> IdAuditor { get; set; } //public int IdAuditor { get; set; }

        public string AuditorLider { get; set; }
    }
}
