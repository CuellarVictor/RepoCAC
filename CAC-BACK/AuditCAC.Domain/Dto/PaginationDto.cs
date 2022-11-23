using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class PaginationDto
    {
        public int NoRegistrosTotales { get; set; }
        public int NoRegistrosTotalesFiltrado { get; set; }
        public int TotalPages { get; set; }
        public dynamic Data { get; set; }
       
    }
}
