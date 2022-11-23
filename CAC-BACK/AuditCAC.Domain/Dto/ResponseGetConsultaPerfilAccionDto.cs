using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseGetConsultaPerfilAccionDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string ActionStatus { get; set; }
    }

    public class ResponseGetConsultaPerfilAccionDetallesDto
    {
        public ResponseGetConsultaPerfilAccionDetallesDto()
        {

        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> ActionStatus { get; set; }
    }

    //public class ListaActionStatus
    //{
    //    public string ActionStatus { get; set; }
    //}
}
