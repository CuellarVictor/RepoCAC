using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class PermisoRol
    {
        public PermisoRol()
        {

        }

        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int? Padre { get; set; }
        public string Img { get; set; }
        public string RolId { get; set; }
        public bool Habilitado { get; set; }
        public bool Visible { get; set; }
        public string User { get; set; }
    }
}
