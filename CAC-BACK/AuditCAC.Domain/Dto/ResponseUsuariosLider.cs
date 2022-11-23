using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseUsuariosLider
    {
        public int IdCobertura { get; set; }
        public int IdMedicion { get; set; }

        [Key]
        public string IdAuditor { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Codigo { get; set; }
        public string NombreUsuario { get; set; }
    }
}
