using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.Actas
{
    public class ParametroDto
    {
        [Key]
        public int Id { get; set; }
        public string ParametroTemplateKey { get; set; }
        public string Value { get; set; }
    }
}
