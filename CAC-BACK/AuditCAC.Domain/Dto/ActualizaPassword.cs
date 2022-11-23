using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ActualizaPassword
    {

        public ActualizaPassword()
        {

        }

        public string Password { get; set; }
        public string Token { get; set; }
    }
}
