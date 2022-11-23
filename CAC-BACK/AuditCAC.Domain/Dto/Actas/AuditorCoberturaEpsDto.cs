using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.Actas
{
    public class AuditorCoberturaEpsDto
    {
        public AuditorCoberturaEpsDto()
        {

        }

        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Funcion { get; set; }
    }
}
