using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistrosAuditoriaModelDto
    
    {
        public string QueryNoRegistrosTotales { get; set; }
        [Key]
        public int Id { get; set; } 
        public int? IdRadicado { get; set; }
        public int? IdMedicion { get; set; }
        public string NombreMedicion { get; set; }
        public string FechaCorteAuditoria { get; set; }        
        public int? IdPeriodo { get; set; }
        public string IdAuditor { get; set; }  //public int? IdAuditor { get; set; } //Guid
        //public string IdLider { get; set; }
        public int? IdEntidad { get; set; }
        public string Entidad { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public string TipoIdentificacion { get; set; } //Varchar de 2 caracteres
        public string Identificacion { get; set; }
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
        //public string EstadoCodigo { get; set; }
        //public string EstadoNombre { get; set; }
        public int? LevantarGlosa { get; set; }
        public int? MantenerCalificacion { get; set; }
        public int? ComiteExperto { get; set; }
        public int? ComiteAdministrativo { get; set; }
        public int? AccionLider { get; set; }
        public int? AccionAuditor { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }
        public bool Encuesta { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ImagePath { get; set; }
        public string NombreAuditor { get; set; }
        public string CodigoUsuario { get; set; }
        public int IdCobertura { get; set; }
        public string EnfermedadMadre { get; set; }
        public string Data_IdEPS { get; set; }
        public int EstadoMedicion { get; set; }
        public string Data_NombreEPS { get; set; }
        public string IPS { get; set; }

        public string FechaApertura { get; set; }
        public string HoraApertura { get; set; }
        public string FechaCierre { get; set; }
        public string HoraCierre { get; set; }
    }
}
