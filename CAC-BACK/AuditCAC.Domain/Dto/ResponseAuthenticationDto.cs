using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseAuthenticationDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public int InactivityTime { get; set; }
        //public int ExpirationTime { get; set; }
        public int SessionDead { get; set; }        
        public objUser objUsuario { get; set; }
        public List<PermisoRol> PermisoRol { get; set; }
        public int CodigoRespuesta { get; set; }
        public string MensajeRespuesta { get; set; }
        public bool EsLider { get; set; }
    }

    public class objUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }        
        public objRol Rol { get; set; }
        public string Name { get; set; }
        public int Codigo { get; set; }
    }

    public class objRol
    {
        public string UserRolName { get; set; }
        public string UserRolId { get; set; }
    }

    //public class ResponseAuthenticationDto
    //{
    //    public string Token { get; set; }
    //    public DateTime Expiration { get; set; }
    //    public int InactivityTime { get; set; }
    //    //public int ExpirationTime { get; set; }
    //    public int SessionDead { get; set; }        
    //    public string UserId { get; set; }
    //    public string UserName { get; set; }
    //    public string UserRolName { get; set; }
    //    public string UserRolId { get; set; }
    //}
}
