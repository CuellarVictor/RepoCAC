using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsCambiarEstadoEntidadDto
    {
        //[Required]
        //public string usuarioId { get; set; }
        [Required]
        public string radicadoId { get; set; }
        [Required]
        public string Observacion { get; set; }
        //[Required]
        //public string estadoAEstado { get; set; }
    }
}
