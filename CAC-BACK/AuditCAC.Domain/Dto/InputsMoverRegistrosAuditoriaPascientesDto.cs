using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsMoverRegistrosAuditoriaPascientesDto
    {
        [Key]
        public int[] Id { get; set; }
        public string IdMedicion { get; set; }
    }
}
