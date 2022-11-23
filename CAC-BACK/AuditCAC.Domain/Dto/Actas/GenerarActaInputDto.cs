using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.Actas
{
    public class GenerarActaInputDto
    {
        public int IdTemplate { get; set; }
        public int IdCobertura { get; set; }
        public string IdEPS { get; set; }

        public IEnumerable<ParametroDto> ListaParametros { get; set; }
    }
}
