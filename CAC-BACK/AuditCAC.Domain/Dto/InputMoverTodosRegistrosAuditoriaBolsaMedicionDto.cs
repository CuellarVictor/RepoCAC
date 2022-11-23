using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputMoverTodosRegistrosAuditoriaBolsaMedicionDto
    {
        [Required]
        public int MedicionIdOriginal { get; set; }
        [Required]
        public int MedicionIdDestino { get; set; }
        public string IdUsuario { get; set; }
    }
}
