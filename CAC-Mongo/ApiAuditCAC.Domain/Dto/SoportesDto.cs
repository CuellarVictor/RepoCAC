using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class SoportesDto
    {
        public SoportesDto()
        {

        }

        public int Id { get; set; }
        public int IdRegistrosAuditoria { get; set; }
        public int IdSoporte { get; set; }
        public string NombreSoporte { get; set; }
        public string UrlSoporte { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Status { get; set; }
    }

}
