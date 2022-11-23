using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    [Keyless]
    public class ResponseDetalleConsultarOrderVariablesDto
    {
        //public int OrdenVariable { get; set; }
        //public int OrdenVxI { get; set; }

        public int OrdenVariableInicial { get; set; }
        public int OrdenVariableFinal { get; set; }
        //
        public int OrdenVxIInicial { get; set; }
        public int OrdenVxIFinal { get; set; }
    }
}
