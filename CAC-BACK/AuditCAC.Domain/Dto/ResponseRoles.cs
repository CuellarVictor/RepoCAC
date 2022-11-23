using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRoles
    {
        public ResponseRoles()
        {

        }

        [key]
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

    }
}
