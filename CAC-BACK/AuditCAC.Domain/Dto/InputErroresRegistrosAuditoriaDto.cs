using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputErroresRegistrosAuditoriaDto
    {
        public int RegistrosAuditoriaDetalleId { get; set; }        
        public string Reducido { get; set; }
        public int? IdRegla { get; set; }
        public int? IdRestriccion { get; set; }
        public int VariableId { get; set; }
        public string ErrorId { get; set; }
        public string Descripcion { get; set; }
        public bool Enable { get; set; }
        public bool NoCorregible { get; set; }        
    }
}
