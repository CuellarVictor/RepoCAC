using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsDuplicarVariablesDto
    {
        [Required]
        public int MedicionIdOrigional { get; set; }
        public int MedicionIdNuevo { get; set; }
        [Required]
        public int UserCreatedBy { get; set; }
        [StringLength(200, ErrorMessage = "El campo no puede tener mas de 200 caracteres!")]
        public string Descripcion { get; set; }
    }
}
