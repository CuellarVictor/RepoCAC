using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    [Keyless]
    public class ResponseSPGetDataMigRegla
    {
        public Int32 idRegla { get;set;}
        public Int32 idCobertura { get; set; }
        public string nombre { get;set;}
        public Byte idTipoRegla { get;set;}
        public Byte idTiempoAplicacion { get; set;}
        public bool habilitado { get; set;}
        public string idError  { get; set;}
        public Nullable<Int32> idVariable { get; set;}
        public Nullable<Byte> idTipoEnvioLimbo { get; set;}
    }
}
