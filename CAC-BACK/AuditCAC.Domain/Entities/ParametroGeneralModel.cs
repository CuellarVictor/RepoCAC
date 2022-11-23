using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{

    [Table("ParametrosGenerales")]
    public class ParametroGeneralModel : BaseEntity
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool Status { get; set; }
    }
}
