using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistrosAuditoriaProgresoDiarioDto
    {
        [Key]
        public string ProgresoDia { get; set; }
        public string Totales { get; set; }
    }
}
