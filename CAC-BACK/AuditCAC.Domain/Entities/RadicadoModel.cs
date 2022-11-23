using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities //AuditCAC.Dal.Entities
{
    public class RadicadoModel
    {
        //[Key]
        //public string idRadicado { get; set; }
        //public int? idArchivoRecibido { get; set; }
        //public DateTime? fechaRadicadoIni { get; set; }
        //public DateTime? fechaRadicadoFin { get; set; }
        //public bool? reemplazado { get; set; }
        //public DateTime? fechaReemplazadoIni { get; set; }
        //public DateTime? fechaReemplazadoFin { get; set; }
        //public string observaciones { get; set; }
        //public string idRadicadoReemplazado { get; set; }
        
        //
        [Key]
        public string idRadicado { get; set; }
        public int? idArchivoRecibido { get; set; }
        public DateTime? fechaRadicado { get; set; }
        public bool? reemplazado { get; set; }
        public DateTime? fechaReemplazado { get; set; }
        public string observaciones { get; set; }
        public string idRadicadoReemplazado { get; set; }
    }
}
