using System;
using System.ComponentModel.DataAnnotations;

namespace AuditCAC.Domain.Dto.UsuarioxEnfermedad
{
    public class UsuarioXEnfermedadDto
    { 
        public int Id { get; set; }
        public int IdCobertura { get; set; }
        public string IdUsuario { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public int Status { get; set; }
    }
}
