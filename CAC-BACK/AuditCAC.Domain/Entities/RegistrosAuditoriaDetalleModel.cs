using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("RegistrosAuditoriaDetalle")]
    public class RegistrosAuditoriaDetalleModel
    {
        [Key]
        public int Id { get; set; }
        public int? VariableId { get; set; }           //Foranea
        public int? RegistrosAuditoriaId { get; set; }
        public int? EstadoVariableId { get; set; }     //Foranea
        public int? Observacion { get; set; }
        public bool? Activo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool EsGlosa { get; set; }
        public bool Visible { get; set; }
        public bool Calificable { get; set; }
        public int? SubgrupoId { get; set; }
        public string DatoReportado { get; set; }
        public string MotivoVariable { get; set; }
        public int? Dato_DC_NC_ND { get; set; }
        public bool? Ara { get; set; }
        public bool Status { get; set; }
    }
}
