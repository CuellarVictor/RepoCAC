using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("RegistrosAuditoria")]
    public class RegistrosAuditoriaModel
    {
        [Key]
        //[Column(TypeName = "bigint")]
        public int Id { get; set; } //public int Id { get; set; }, long, Int64
        public int? IdRadicado { get; set; }
        public int? IdMedicion { get; set; }
        public int? IdPeriodo { get; set; }
        public string IdAuditor { get; set; }  //public int? IdAuditor { get; set; } //Guid
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public string TipoIdentificacion { get; set; } //Varchar de 2 caracteres
        public string Identificacion { get; set; }  //int?
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaAuditoria { get; set; }

        public DateTime? FechaMinConsultaAudit { get; set; }
        public DateTime? FechaMaxConsultaAudit { get; set; }

        public DateTime? FechaAsignacion { get; set; }

        public bool Activo { get; set; }
        public string Conclusion { get; set; }
        public string UrlSoportes { get; set; }
        public bool Reverse { get; set; }
        public int? DisplayOrder { get; set; }
        public bool Ara { get; set; }
        public bool Eps { get; set; }
        public DateTime? FechaReverso { get; set; }
        public bool AraAtendido { get; set; }
        public bool EpsAtendido { get; set; }
        public bool Revisar { get; set; }
        public bool Extemporaneo { get; set; }
        public int? Estado { get; set; }
        public int? LevantarGlosa { get; set; }
        public int? MantenerCalificacion { get; set; }
        public int? ComiteExperto { get; set; }
        public int? ComiteAdministrativo { get; set; }
        public int? AccionLider { get; set; }
        public int? AccionAuditor { get; set; }
        public bool Encuesta { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Status { get; set; }
    }
}
