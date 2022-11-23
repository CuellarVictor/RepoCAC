using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistrosAuditoriaDetallesAsignacionDto
    {
        [Key]
        public int Order { get; set; }
        public string Nombre { get; set; }
        public string NoRegistros { get; set; }
        public int Extemporaneo { get; set; }
        public string Estados { get; set; }
    }
}
