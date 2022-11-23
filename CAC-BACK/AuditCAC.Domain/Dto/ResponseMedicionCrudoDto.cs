using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseMedicionCrudoDto
    {
        //public string QueryNoRegistrosTotales { get; set; }
        //public string Id { get; set; }
        //public string IdCobertura { get; set; }
        //public string IdPeriodo { get; set; }
        //public string Descripcion { get; set; }
        //public string Activo { get; set; }
        //public string CreatedBy { get; set; }
        //public string CreatedDate { get; set; }
        //public string ModifyBy { get; set; }
        //public string ModifyDate { get; set; }

        public string QueryNoRegistrosTotales { get; set; }
        public int? Id { get; set; }
        public int? IdCobertura { get; set; }
        public int? IdPeriodo { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
