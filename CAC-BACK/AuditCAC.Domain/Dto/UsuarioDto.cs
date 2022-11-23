using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class UsuarioDto
    {
        public UsuarioDto()
        {

        }

        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Telefono { get; set; }
        public int RolId { get; set; }
        public string NormalizedName { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Estado { get; set; }
        public string NombreEstado { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyPasswordDate { get; set; }
        public bool Enable { get; set; }
    }
}
