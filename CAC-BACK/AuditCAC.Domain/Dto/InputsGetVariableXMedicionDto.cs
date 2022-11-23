using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsGetVariableXMedicionDto
    {
        [Required]
        public string VariableId { get; set; }
        [Required]
        public string MedicionId { get; set; }
    }
}
