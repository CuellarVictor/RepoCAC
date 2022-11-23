using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseAutenticacionDto
    {
        [Key]
        public int Id { get; set; }
        public string MensajeRespuesta { get; set; }
        public string UserId { get; set; }
        public string TokenBloqueo { get; set; }
        public string RolId { get; set; }
        public string NombreRol { get; set; }
        public string Usuario { get; set; }
        public string Nombres { get; set; }
        public string Email { get; set; }
        public int Codigo { get; set; }
        public bool? EsLider { get; set; }
    }
}
