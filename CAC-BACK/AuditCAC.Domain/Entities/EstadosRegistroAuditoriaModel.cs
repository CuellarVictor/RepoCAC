using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("EstadosRegistroAuditoria")]
    public class EstadosRegistroAuditoriaModel
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripción { get; set; }
        public bool Status { get; set; }
    }
}
