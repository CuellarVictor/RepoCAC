using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.BolsasMedicion
{
    public class BolsaMedicionNuevaDto
    {
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
        public DateTime? FechaInicioAuditoria { get; set; } //string
        public DateTime? FechaFinAuditoria { get; set; } //string
        public DateTime? FechaCorteAuditoria { get; set; } //string
        public string Lider { get; set; }
        public string Resolucion { get; set; }
        public string Nombre { get; set; }
        public bool Status { get; set; }
    }
}
