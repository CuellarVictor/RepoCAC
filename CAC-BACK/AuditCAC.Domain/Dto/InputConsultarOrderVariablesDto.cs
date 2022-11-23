using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputConsultarOrderVariablesDto
    {
        public string Variable { get; set; }
        public int idCobertura { get; set; }
        public string MedicionId { get; set; }
    }
}
