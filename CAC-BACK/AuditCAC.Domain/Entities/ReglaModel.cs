using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities //AuditCAC.Dal.Entities
{
    public class ReglaModel
    {
        [Key]
        public int? idRegla { get; set; }
        public int? idCobertura { get; set; }
        public string nombre { get; set; }
        public byte? idTipoRegla { get; set; }
        public byte? idTiempoAplicacion { get; set; }
        public bool? habilitado { get; set; }
        public string idError { get; set; }
        public int? idVariable { get; set; }
        public byte? idTipoEnvioLimbo { get; set; }
    }
}
