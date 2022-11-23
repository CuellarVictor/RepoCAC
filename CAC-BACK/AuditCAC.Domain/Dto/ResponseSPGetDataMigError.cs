using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuditCAC.Domain.Dto
{
    [Keyless]
    public class ResponseSPGetDataMigError
    {
        public string idError { get; set; }
        public String descripcion { get; set; }
        public String idTipoError { get; set; }
    }
}
