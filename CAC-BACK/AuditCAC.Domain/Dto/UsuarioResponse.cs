using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class UsuarioResponse
    {
        public UsuarioResponse()
        {
            this.UsuariosxEnfermedad = new List<int>();
        }

        public UsuarioResponse(UsuarioDto dto, List<UsuarioxEnfermedadDto> usuarioxenfermedadDto)
        {
            this.Id = dto.Id;
            this.Usuario = dto.Usuario;
            this.Email = dto.Email;
            this.Password = dto.Password;
            this.Telefono = dto.Telefono;
            this.RolId = dto.RolId;
            this.NormalizedName = dto.NormalizedName;
            this.Codigo = dto.Codigo;
            this.Nombre = dto.Nombre;
            this.Apellidos = dto.Apellidos;
            this.Estado = dto.Estado;
            this.NombreEstado = dto.NombreEstado;
            this.CreatedDate = dto.CreatedDate;
            this.ModifyDate = dto.ModifyDate;
            this.ModifyPasswordDate = dto.ModifyPasswordDate;
            this.Enable = dto.Enable;
            this.UsuariosxEnfermedad = new List<int>();

            this.UsuariosxEnfermedad = usuarioxenfermedadDto.Where(x => x.IdUsuario == this.Id).Select(x => x.IdCobertura).ToList();
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
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyPasswordDate { get; set; }
        public bool Enable { get; set; }
        public List<int> UsuariosxEnfermedad { get; set; }
        
    }
}
