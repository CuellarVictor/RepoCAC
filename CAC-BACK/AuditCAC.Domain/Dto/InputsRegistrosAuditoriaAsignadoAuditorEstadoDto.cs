using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsRegistrosAuditoriaAsignadoAuditorEstadoDto
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        //
        public string IdAuditor { get; set; }
        public string Estado { get; set; }
    }
}
