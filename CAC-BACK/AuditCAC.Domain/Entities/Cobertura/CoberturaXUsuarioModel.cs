using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AuditCAC.Domain.Entities.Cobertura
{
    public class CoberturaXUsuarioModel
    {
        [Key]
        public Guid Idk { get; set; }
        public int idCobertura { get; set; } //int?    
        public string nombre { get; set; } //int?
        public string idUsuario { get; set; } //int?    
    }
}
