using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("Medicion")]
    public class MedicionModel
    {       
        [Key]
        public int Id { get; set; } 
        public int? IdCobertura { get; set; }
        public int? IdPeriodo { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public int Estado { get; set; }
        public DateTime? FechaInicioAuditoria { get; set; }
        public DateTime? FechaFinAuditoria { get; set; }
        public DateTime? FechaCorteAuditoria { get; set; }
        public string Lider { get; set; }
        public string Resolucion { get; set; }
        public string Nombre { get; set; }
        public bool Status { get; set; } = true;
    }
}
