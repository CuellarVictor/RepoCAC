using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseUsuariosBolsaMedicionFiltroDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Codigo { get; set; } //int
    }
}
